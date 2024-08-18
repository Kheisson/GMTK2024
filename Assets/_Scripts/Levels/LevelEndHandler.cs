using System;
using System.Collections.Generic;
using Player;
using UnityEngine;

namespace Levels
{
    public class LevelEndHandler : MonoBehaviour
    {
        private Dictionary<EPlayerType, bool> playerStates = new Dictionary<EPlayerType, bool>()
        {
            [EPlayerType.X] = false,
            [EPlayerType.Y] = false
        };
        
        public void NotifyPlayerStateChanged(EPlayerType playerType, bool hasFinished)
        {
            playerStates[playerType] = hasFinished;
            CheckForLevelTransition();
        }

        private void CheckForLevelTransition()
        {
            foreach (EPlayerType type in Enum.GetValues(typeof(EPlayerType)))
            {
                if (!playerStates.TryGetValue(type, out bool hasFinished) || !hasFinished)
                {
                    return;
                }
            }

            StartLevelTransition();
        }

        private void StartLevelTransition()
        {
        }
    }
}