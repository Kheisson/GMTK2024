using _Scripts.Audio;
using _Scripts.Scaling;
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
            var scalerManager = new ScalerManager();
            var sceneLoader = new SceneLoader();

            ServiceLocator.RegisterService(audioManager);
            ServiceLocator.RegisterService(popupManager);
            ServiceLocator.RegisterService(scalerManager);
            ServiceLocator.RegisterService(sceneLoader);
            
            await InitializeUiManagerAsync();
        }

        private async UniTask InitializeUiManagerAsync()
        {
            var uiManagerInstance = Instantiate(uiManagerPrefab).GetComponent<UiManager>();
            await uiManagerInstance.Initialize();
            ServiceLocator.RegisterService(uiManagerInstance);
            uiManagerInstance.CheckIfTitleScene();
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
