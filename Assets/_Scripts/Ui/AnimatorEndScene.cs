using _Scripts.Infra;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts.Ui
{
    public class AnimatorEndScene : MonoBehaviour
    {
        [SerializeField] private Button startButton;
        
        private void Start()
        {
            startButton.onClick.AddListener(() =>
            {
                ServiceLocator.GetService<SceneLoader>().LoadNextScene().Forget();
            });
        }
    }
}