using System;
using Player;

namespace Controls
{
    public class KeyBindingsManager
    {
        public bool IsSharedBindings { get; set; } = true;
        public event Action<bool> OnBindingsChanged;

        public EPlayerType CurrentActivePlayer { get; set; }
        public event Action<EPlayerType> OnActivePlayerChanged;
        public void SwitchPlayer()
        {
            CurrentActivePlayer = CurrentActivePlayer == EPlayerType.X
                ? EPlayerType.Y
                : EPlayerType.X;
            
            OnActivePlayerChanged?.Invoke(CurrentActivePlayer);
        }
    }
}