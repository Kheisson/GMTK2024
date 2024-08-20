using _Scripts.Infra;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Scripts.Ui.Popups
{
    public class LayoutPopup : PopupView
    {
        [SerializeField] private Button _singlePlayerButton;
        [SerializeField] private Button _multiPlayerButton;
        
        private void Start()
        {
            _singlePlayerButton.onClick.AddListener(OnSinglePlayerButtonClicked);
            _multiPlayerButton.onClick.AddListener(OnMultiPlayerButtonClicked);
        }
        
        private async void OnSinglePlayerButtonClicked()
        {
            PlayerPrefs.SetInt("IsSinglePlayer", 1);
            PlayerPrefs.Save();
            await LoadScene();
        }
        
        private async void OnMultiPlayerButtonClicked()
        {
            PlayerPrefs.SetInt("IsSinglePlayer", 0);
            PlayerPrefs.Save();
            await LoadScene();
        }

        private async UniTask LoadScene()
        {
            var sceneLoader = ServiceLocator.GetService<SceneLoader>();
            
            if (GameContainer.Instance.InGameplayScene)
            {
                await sceneLoader.ReloadCurrentScene();
            }
            else if (SceneManager.GetActiveScene().buildIndex == 1) //Cutscene
            {
                await ServiceLocator.GetService<PopupManager>().ClosePopupAsync();
                Time.timeScale = 1;
                return;
            }
            else
            {
                await sceneLoader.LoadNextScene();
            }
            
            await ServiceLocator.GetService<PopupManager>().ClosePopupAsync();
        }
    }
}