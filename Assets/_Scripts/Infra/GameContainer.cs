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

        private async UniTask InitializeServices()
        {
            var audioManager = new AudioManager(audioMixer);
            var popupManager = new PopupManager(popupCollection);

            ServiceLocator.RegisterService(audioManager);
            ServiceLocator.RegisterService(popupManager);

            audioManager.LoadSettings();
            
            await InitializeUiManagerAsync();
        }

        private async UniTask InitializeUiManagerAsync()
        {
            var uiManagerInstance = Instantiate(uiManagerPrefab, transform).GetComponent<UiManager>();
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
