using System;
using System.Collections.Generic;
using _Scripts.Infra;
using Cysharp.Threading.Tasks;
using Player;
using UnityEngine;

namespace Levels
{
    public class LevelEndHandler : MonoBehaviour
    {
        [SerializeField] private float levelEndDelaySeconds = 0.5f;
        private bool _hasLevelFinished;
        private Dictionary<EPlayerType, bool> _playerStates;
        public event Action OnLevelEnded;

        private void Awake()
        {
            _playerStates = new Dictionary<EPlayerType, bool>()
            {
                [EPlayerType.X] = false,
                [EPlayerType.Y] = false
            };
        }

        public void NotifyPlayerStateChanged(EPlayerType playerType, bool hasFinished)
        {
            _playerStates[playerType] = hasFinished;
            CheckForLevelTransition();
        }

        private void CheckForLevelTransition()
        {
            if (_hasLevelFinished)
            {
                return;
            }
            
            foreach (EPlayerType type in Enum.GetValues(typeof(EPlayerType)))
            {
                if (!_playerStates.TryGetValue(type, out bool hasFinished) || !hasFinished)
                {
                    return;
                }
            }

            _hasLevelFinished = true;
            StartLevelTransition();
        }

        private void StartLevelTransition()
        {
            OnLevelEnded?.Invoke();
            UniTask.WaitForSeconds(levelEndDelaySeconds).ContinueWith(() =>
            {
                ServiceLocator.GetService<SceneLoader>().LoadNextScene().Forget();
            });
        }
    }
}