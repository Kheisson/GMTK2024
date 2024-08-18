using _Scripts.Infra;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Ui.Popups
{
    public class LevelEndPopup : PopupView
    {
        [SerializeField] private Button restartButton;
        [SerializeField] private GameObject playerX;
        [SerializeField] private GameObject playerY;

        private void Start()
        {
            restartButton.onClick.AddListener(OnRestartButtonClick);
        }

        private void OnEnable()
        {
            if (string.IsNullOrEmpty(Metadata))
            {
                Debug.LogError("Metadata is not set.");
            }
            else
            {
                if (Metadata.Equals(EPlayerType.X.ToString()))
                {
                    playerX.SetActive(true);
                }
                else if (Metadata.Equals(EPlayerType.Y.ToString()))
                {
                    playerY.SetActive(true);
                }
                else
                {
                    Debug.LogError("Invalid metadata.");
                }
            }
        }

        private async void OnRestartButtonClick()
        {
            var sceneLoader = ServiceLocator.GetService<SceneLoader>();
            await sceneLoader.ReloadCurrentScene();
        }
    }
}