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

        public async UniTask ShowAsync()
        {
            canvasGroup.alpha = 0;
            popupTransform.anchoredPosition = new Vector2(0, Screen.height * 2f);
            
            await canvasGroup.DOFade(1, FADE_DURATION).SetEase(Ease.InQuad);
            await popupTransform.DOAnchorPos(Vector2.zero, MOVE_DURATION).SetEase(Ease.OutBack).AsyncWaitForCompletion();
        }

        public async UniTask HideAsync()
        {
            await popupTransform.DOAnchorPos(new Vector2(0, -Screen.height * 2f), MOVE_DURATION).SetEase(Ease.InBack).AsyncWaitForCompletion();
            await canvasGroup.DOFade(0, FADE_DURATION).SetEase(Ease.OutQuad);
        }
    }
}