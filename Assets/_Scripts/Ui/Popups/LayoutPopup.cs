using _Scripts.Infra;
using Cysharp.Threading.Tasks;
using UnityEngine;
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
        
        private void OnSinglePlayerButtonClicked()
        {
            PlayerPrefs.SetInt("IsSinglePlayer", 1);
            PlayerPrefs.Save();
            LoadScene();
        }
        
        private void OnMultiPlayerButtonClicked()
        {
            PlayerPrefs.SetInt("IsSinglePlayer", 0);
            PlayerPrefs.Save();
            LoadScene();
        }

        private void LoadScene()
        {
            if (GameContainer.Instance.InGameplayScene)
            {
                ServiceLocator.GetService<SceneLoader>().ReloadCurrentScene().Forget();
            }
            else
            {
                ServiceLocator.GetService<SceneLoader>().LoadNextScene().Forget();
            }
        }
    }
}