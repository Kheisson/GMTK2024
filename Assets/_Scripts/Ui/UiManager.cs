using _Scripts.Infra;
using _Scripts.Ui.Popups;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Scripts.Ui
{
    public class UiManager : MonoBehaviour
    {
        private const string TITLE_SCENE_NAME = "TitleScene";
        private const string UI_SCENE_NAME = "UiScene";
        [SerializeField] private Canvas hudCanvas;
        [SerializeField] private Canvas settingsCanvas;

        public async UniTask Initialize()
        {
            await LoadUiSceneAsync();
            
            if (settingsCanvas != null)
            {
                ServiceLocator.GetService<PopupManager>().OnPopupOpen += AddBlackBackgroundScreen;
                ServiceLocator.GetService<PopupManager>().OnPopupClose += RemoveBlackBackgroundScreen;
            }
            
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private async UniTask LoadUiSceneAsync()
        {
            var loadOperation = SceneManager.LoadSceneAsync(UI_SCENE_NAME, LoadSceneMode.Additive);
            await AwaitLoadScene(loadOperation);
            
            if (hudCanvas == null || settingsCanvas == null)
            {
                Debug.LogError("Failed to find HUD or Settings canvas in UiScene.");
            }
        }

        private async UniTask AwaitLoadScene(AsyncOperation loadOperation)
        {
            while (!loadOperation.isDone)
            {
                await UniTask.Yield();
            }
        }

        public Canvas GetHudCanvas()
        {
            return hudCanvas;
        }

        public Canvas GetSettingsCanvas()
        {
            return settingsCanvas;
        }

        private void AddBlackBackgroundScreen()
        {
            var blackBackground = new GameObject("BlackBackground", typeof(Image));
            blackBackground.transform.SetParent(settingsCanvas.transform);
            blackBackground.transform.SetAsFirstSibling();
            blackBackground.GetComponent<Image>().color = new Color(0, 0, 0, 0.75f);
            blackBackground.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
            blackBackground.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        
        private void RemoveBlackBackgroundScreen()
        {
            var blackBackground = settingsCanvas.transform.Find("BlackBackground");
            
            if (blackBackground != null)
            {
                Destroy(blackBackground.gameObject);
            }
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            CheckIfTitleScene();
        }
        
        public void CheckIfTitleScene()
        {
            const string startMenuName = "StartMenu";
            
            var startMenu = hudCanvas.transform.Find(startMenuName);

            startMenu.gameObject.SetActive(SceneManager.GetActiveScene().name == TITLE_SCENE_NAME);
        }
    }
}