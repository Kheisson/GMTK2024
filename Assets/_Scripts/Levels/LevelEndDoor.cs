using System;
using _Scripts.Audio;
using _Scripts.Infra;
using Animations;
using Controls;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Player;
using UnityEngine;

namespace Levels
{
    public class LevelEndDoor : MonoBehaviour
    {
        [SerializeField] private EPlayerType playerType;
        [SerializeField] private LevelEndHandler levelEndHandler;
        [SerializeField] private float scaleDuration = 0.5f;
        [SerializeField] private float exitDelay = 0.2f;

        private bool _hasFinished;
        
        private Animator _animator;
        private Transform _playerTransform;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            levelEndHandler.OnLevelEnded += AnimateOut;
        }

        private void OnDisable()
        {
            levelEndHandler.OnLevelEnded -= AnimateOut;
        }

        private void AnimateOut()
        {
            if (_playerTransform != null)
            {
                PlayOutAnimation().Forget();
                _playerTransform.SetParent(transform);
                _hasFinished = true;
            }
        }

        private async UniTask PlayOutAnimation()
        {
            _playerTransform.SetParent(transform);
            await UniTask.Delay(TimeSpan.FromSeconds(exitDelay));
            ServiceLocator.GetService<AudioManager>().PlaySfx(AudioConstants.WIN);
            await transform.DOScale(Vector3.zero, scaleDuration).SetEase(Ease.InOutBounce);
            gameObject.SetActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_hasFinished)
            {
                return;
            }
            var player = other.GetComponent<PlayerController>();
        
            if (player != null && player.PlayerType == playerType)
            {
                _playerTransform = player.transform;
                levelEndHandler.NotifyPlayerStateChanged(playerType, true);
                _animator.SetBool(AnimationConstants.IS_PLATFORM_ON_KEY, true);
                ServiceLocator.GetService<AudioManager>().PlaySfx(AudioConstants.PLATFORM_ON);
            }
        }
    
        private void OnTriggerExit2D(Collider2D other)
        {
            if (_hasFinished)
            {
                return;
            }
            
            var player = other.GetComponent<PlayerController>();

            if (player != null && player.PlayerType == playerType)
            {
                levelEndHandler.NotifyPlayerStateChanged(playerType, false);
                _animator.SetBool(AnimationConstants.IS_PLATFORM_ON_KEY, false);
                ServiceLocator.GetService<AudioManager>().PlaySfx(AudioConstants.PLATFORM_OFF);
            }
        }
    }
}

