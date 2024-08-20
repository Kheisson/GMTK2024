using _Scripts.Audio;
using _Scripts.Scaling;
using _Scripts.Ui;
using _Scripts.Ui.Popups;
using Controls;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cysharp.Threading.Tasks;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

namespace _Scripts.Infra
{
    public class GameContainer : MonoBehaviour
    {
        public static GameContainer Instance { get; private set; }

        public PopupCollection popupCollection;
        public AudioMixer audioMixer;
        public GameObject uiManagerPrefab;

        private PlayerController _playerX;
        private PlayerController _playerY;
        private PlayerController _currentPlayer;

        private bool IsSinglePlayer => PlayerPrefs.GetInt("IsSinglePlayer") == 1;
        public bool InGameplayScene => SceneManager.GetActiveScene().buildIndex >= 3;

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

        private void Update()
        {
            if (!IsSinglePlayer) return;
            
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SwitchControl();
            }
        }

        private async UniTask InitializeServices()
        {
            var audioManager = new AudioManager(audioMixer, GetComponentInChildren<AudioSource>());
            var popupManager = new PopupManager(popupCollection);
            var scalerManager = new ScalerManager();
            var sceneLoader = new SceneLoader();
            SceneManager.sceneLoaded += OnSceneLoaded;

            ServiceLocator.RegisterService(audioManager);
            ServiceLocator.RegisterService(popupManager);
            ServiceLocator.RegisterService(scalerManager);
            ServiceLocator.RegisterService(sceneLoader);
            
            await InitializeUiManagerAsync();
        }

        private async UniTask InitializeUiManagerAsync()
        {
            var uiManagerInstance = Instantiate(uiManagerPrefab, transform).GetComponent<UiManager>();
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
        
        private void SetupPlayers()
        {
            if (InGameplayScene && IsSinglePlayer)
            {
                Debug.Log("Setting up players singleplayer");

                _playerX = GameObject.Find("PlayerX").GetComponent<PlayerController>();
                _playerY = GameObject.Find("PlayerY").GetComponent<PlayerController>();

                _playerX.GetComponent<PlayerInput>().enabled = false;
                _playerY.GetComponent<PlayerInput>().enabled = false;
                
                _playerX?.InitializeInput("PlayerX");
                _playerY?.InitializeInput("PlayerX");

                _currentPlayer = _playerX;
                
                if (_currentPlayer != null)
                {
                    _currentPlayer.SetAsCurrentPlayer();
                    Debug.Log($"Initial current player: {_currentPlayer.name}");
                }
            } else if (InGameplayScene && !IsSinglePlayer)
            {
                Debug.Log("Setting up players multiplayer");

                _playerX = GameObject.Find("PlayerX").GetComponent<PlayerController>();
                _playerY = GameObject.Find("PlayerY").GetComponent<PlayerController>();

                _playerX.GetComponent<PlayerInput>().enabled = true;
                _playerY.GetComponent<PlayerInput>().enabled = true;
                
                _playerX?.InitializeInput("PlayerX");
                _playerY?.InitializeInput("PlayerY");
            }
        }
        
        private void SwitchControl()
        {
            if (_currentPlayer != null)
            {
                _currentPlayer.OnSetInactive();
            }

            _currentPlayer = _currentPlayer == _playerX ? _playerY?.SetAsCurrentPlayer() : _playerX?.SetAsCurrentPlayer();
            Debug.Log($"Current player after switch: {_currentPlayer?.name}");
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (InGameplayScene && mode == LoadSceneMode.Single)
            {
                SetupPlayers();
            }
        }
    }
}
