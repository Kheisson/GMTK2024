using System;
using System.Collections.Generic;
using Controls;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Player;
using UnityEngine;

namespace Tutorial
{
    public class TutorialStep : MonoBehaviour
    {
        [SerializeField] private List<EPlayerType> playerTypes;
        [SerializeField] private GameObject stepObject;
        [SerializeField] private float hideDuration = 0.5f;
        [SerializeField] private float showDuration = 0.5f;
        private bool _wasTriggered = false;

        public event Action OnStepTriggered;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_wasTriggered)
            {
                return;
            }
            
            if (other.TryGetComponent<PlayerController>(out PlayerController player))
            {
                if (playerTypes.Contains(player.PlayerType))
                {
                    _wasTriggered = true;
                    OnStepTriggered?.Invoke();
                }
            }
        }

        public async UniTask Hide()
        {
            await stepObject.transform.DOScale(Vector3.zero, hideDuration).SetEase(Ease.InOutBounce);
            stepObject.SetActive(false);
        }

        public async UniTask Show()
        {
            stepObject.SetActive(true);
            stepObject.transform.localScale = Vector3.zero;
            await stepObject.transform.DOScale(Vector3.one, showDuration).SetEase(Ease.InOutBounce);
        }

        public void SetInactive()
        {
            stepObject.SetActive(false);
        }
    }
}