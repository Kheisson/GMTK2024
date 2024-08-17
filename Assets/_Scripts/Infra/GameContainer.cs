using _Scripts.Audio;
using _Scripts.Ui;
using _Scripts.Ui.Popups;
using UnityEngine;
using UnityEngine.Audio;
using Cysharp.Threading.Tasks;

namespace _Scripts.Infra
{
    public class GameContainer : MonoBehaviour
    {
        public static GameContainer Instance { get; private set; }

        public PopupCollection popupCollection;
        public AudioMixer audioMixer;
        public GameObject uiManagerPrefab;

        private async void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                await InitializeServices();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            ServiceLocator.GetService<AudioManager>().LoadSettings();
        }

        private async UniTask InitializeServices()
        {
            var audioManager = new AudioManager(audioMixer);
            var popupManager = new PopupManager(popupCollection);

            ServiceLocator.RegisterService(audioManager);
            ServiceLocator.RegisterService(popupManager);
            
            await InitializeUiManagerAsync();
        }

        private async UniTask InitializeUiManagerAsync()
        {
            var uiManagerInstance = Instantiate(uiManagerPrefab).GetComponent<UiManager>();
            await uiManagerInstance.Initialize();
            ServiceLocator.RegisterService(uiManagerInstance);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnGameLoad()
        {
            _ = LoadAsync();
        }

        private static async UniTask LoadAsync()
        {
            if (Instance == null)
            {
                var containerPrefab = await Resources.LoadAsync<GameObject>(nameof(GameContainer)) as GameObject;
                
                if (containerPrefab != null)
                {
                    Instantiate(containerPrefab);
                }
                else
                {
                    Debug.LogError("Failed to load GameContainer prefab.");
                }
            }
        }
    }
}
