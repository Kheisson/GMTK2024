using _Scripts.Infra;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Ui
{
    public class StartTitleView : MonoBehaviour
    {
        [SerializeField] private Button _startButton;
        private SceneLoader _sceneLoader;
        
        private void Start()
        {
            _sceneLoader = ServiceLocator.GetService<SceneLoader>();
            _startButton.onClick.AddListener(OnStartButtonClick);
        }
        
        private void OnStartButtonClick()
        {
            var lastPlayedSceneIndex = _sceneLoader.LoadProgress();
            
            if (lastPlayedSceneIndex > 2)
            {
                _sceneLoader.LoadSceneAsync(lastPlayedSceneIndex).Forget();
            }
            else
            {
                _sceneLoader.LoadNextScene().Forget();
            }
        }
    }
}