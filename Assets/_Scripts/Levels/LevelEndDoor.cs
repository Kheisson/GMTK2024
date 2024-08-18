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
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            var player = other.GetComponent<PlayerController>();
        
            if (player != null && player.PlayerType == playerType)
            {
                levelEndHandler.NotifyPlayerStateChanged(playerType, true);
            }
        }
    
        private void OnTriggerExit2D(Collider2D other)
        {
            var player = other.GetComponent<PlayerController>();
        
            if (player != null && player.PlayerType == playerType)
            {
                levelEndHandler.NotifyPlayerStateChanged(playerType, false);
            }
        }
    }
}

