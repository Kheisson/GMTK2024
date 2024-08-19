using System;
using _Scripts.Audio;
using _Scripts.Infra;
using Animations;
using Controls;
using Player;
using UnityEngine;

namespace Levels
{
    public class LevelEndDoor : MonoBehaviour
    {
        [SerializeField] private EPlayerType playerType;
        [SerializeField] private LevelEndHandler levelEndHandler;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.GetComponent<PlayerController>();
        
            if (player != null && player.PlayerType == playerType)
            {
                levelEndHandler.NotifyPlayerStateChanged(playerType, true);
                _animator.SetBool(AnimationConstants.IS_PLATFORM_ON_KEY, true);
                ServiceLocator.GetService<AudioManager>().PlaySfx(AudioConstants.PLATFORM_ON);
            }
        }
    
        private void OnTriggerExit2D(Collider2D other)
        {
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

