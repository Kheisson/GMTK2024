using UnityEngine;
using UnityEngine.InputSystem;

namespace Controls
{
    public class PlayerInputHandler 
    {
        private readonly PlayerControls _playerControls;
        
        public PlayerInputHandler()
        { 
            _playerControls = new PlayerControls();

            _playerControls.Gameplay.HorizontalMovement.performed += OnMovePerformed;
            _playerControls.Gameplay.Jump.performed += OnJumpPerformed;
        }
        
        public void Enable()
        {
            _playerControls.Enable();
        }

        public void Disable()
        {
            _playerControls.Disable();
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            var rawInput = context.ReadValue<Vector2>();
            NormInputX = Mathf.RoundToInt(rawInput.x);
        }
        
        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            
        }

        public int NormInputX { get; private set; }

        ~PlayerInputHandler()
        {
            _playerControls.Gameplay.HorizontalMovement.performed -= OnMovePerformed;
            _playerControls.Gameplay.HorizontalMovement.performed -= OnJumpPerformed;
        }
    }
}