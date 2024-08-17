using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Ui
{
    public class UiManager : MonoBehaviour
    {
        private const string UI_SCENE_NAME = "UiScene";
        [SerializeField] private Canvas hudCanvas;
        [SerializeField] private Canvas settingsCanvas;

        public async UniTask Initialize()
        {
            await LoadUiSceneAsync();
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
    }
}