using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Scripts.Ui.Popups
{
    public class PopupView : MonoBehaviour
    {
        private const float FADE_DURATION = 0.5f;
        private const float MOVE_DURATION = 0.75f;
        [SerializeField] private RectTransform popupTransform;
        [SerializeField] private CanvasGroup canvasGroup;
        protected string Metadata { get; private set; }

        public async UniTask ShowAsync()
        {
            canvasGroup.alpha = 0;
            popupTransform.anchoredPosition = new Vector2(0, Screen.height * 2f);
            
            var fadeTask = canvasGroup.DOFade(1, FADE_DURATION).SetEase(Ease.InQuad).AsyncWaitForCompletion().AsUniTask();
            var moveTask = popupTransform.DOAnchorPos(Vector2.zero, MOVE_DURATION).SetEase(Ease.OutBack).AsyncWaitForCompletion().AsUniTask();
            
            await UniTask.WhenAll(fadeTask, moveTask);
        }

        public async UniTask HideAsync()
        {
            var moveTask = popupTransform.DOAnchorPos(new Vector2(0, -Screen.height * 2f), MOVE_DURATION).SetEase(Ease.InBack).AsyncWaitForCompletion().AsUniTask();
            var fadeTask = canvasGroup.DOFade(0, FADE_DURATION).SetEase(Ease.OutQuad).AsyncWaitForCompletion().AsUniTask();
            
            await UniTask.WhenAll(moveTask, fadeTask);
        }
        
        public void SetMetadata(string metadata)
        {
            gameObject.SetActive(false);
            Metadata = metadata;
            gameObject.SetActive(true);
        }
    }
}