using DG.Tweening;
using UnityEngine;

namespace _Scripts.Ui
{
    public class ArrowAnimation : MonoBehaviour
    {
        private void OnEnable()
        {
            transform.DOLocalMoveY(transform.localPosition.y + 0.1f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        }
        
        private void OnDisable()
        {
            transform.DOKill();
        }
    }
}