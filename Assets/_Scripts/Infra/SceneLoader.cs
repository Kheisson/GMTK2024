using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Scripts.Infra
{
    public class SceneLoader
    {
        private const string LAST_SCENE_KEY = "LastSceneIndex";

        public async UniTaskVoid LoadSceneAsync(int sceneIndex)
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
    }
}