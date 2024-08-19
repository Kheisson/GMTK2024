using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Infra
{
    public class SceneLoader
    {
        private const string LAST_SCENE_KEY = "LastSceneIndex";
        private const int UI_SCENE_INDEX = 2;
        
        public async UniTask LoadSceneAsync(int sceneIndex)
        {
            var loadOperation = SceneManager.LoadSceneAsync(sceneIndex);
            
            while (loadOperation != null && !loadOperation.isDone)
            {
                await UniTask.Yield();
            }
        }

        public void SaveProgress(int sceneIndex)
        {
            PlayerPrefs.SetInt(LAST_SCENE_KEY, sceneIndex);
            PlayerPrefs.Save();
        }

        public int LoadProgress()
        {
            return PlayerPrefs.GetInt(LAST_SCENE_KEY, 0);
        }

        public bool HasNextScene()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            return currentSceneIndex < SceneManager.sceneCountInBuildSettings - 1;
        }
        
        public async UniTask ReloadCurrentScene()
        {
            Time.timeScale = 1;
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            await LoadSceneAsync(currentSceneIndex);
        }

        public async UniTask LoadNextScene()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;

            // Skip the UI scene
            if (nextSceneIndex == UI_SCENE_INDEX)
            {
                nextSceneIndex++;
            }

            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SaveProgress(nextSceneIndex);
                await LoadSceneAsync(nextSceneIndex);
            }
            else
            {
                Debug.LogError("No more scenes to load, going back to TitleScene");
                DeleteProgress();
                await LoadSceneAsync(0);
            }
        }

        private static void DeleteProgress()
        {
            PlayerPrefs.DeleteKey(LAST_SCENE_KEY);
            PlayerPrefs.Save();
        }
    }
}
