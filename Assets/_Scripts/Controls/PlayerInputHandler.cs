using UnityEngine;
using UnityEngine.InputSystem;

namespace Controls
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private float inputStayTime;
        
        public int NormInputX { get; private set; }
        public bool JumpInput { get; private set; }
        public bool JumpInputStop { get; private set; }
        public bool IsScaleUpInput { get; private set; }
        public bool IsScaleDownInput { get; private set; }

        
        private float _jumpInputStartTime;

        public PlayerInputHandler(bool isScaleUpInput)
        {
            IsScaleUpInput = isScaleUpInput;
        }

        // invoked via unity event
        public void OnMovePerformed(InputAction.CallbackContext context)
        {
            var rawInput = context.ReadValue<Vector2>();
            NormInputX = Mathf.RoundToInt(rawInput.x);
        }
        
        // invoked via unity event
        public void OnJumpPerformed(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                JumpInput = true;
                JumpInputStop = false;
                _jumpInputStartTime = Time.time;
            }
            
            if (context.canceled)
            {
                JumpInputStop = true;
            }
        }
        
        public void OnScaleUpPerformed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                IsScaleUpInput = true;
            }
            else if (context.canceled)
            {
                IsScaleUpInput = false;
            }
        }
        
        public void OnScaleDownPerformed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                IsScaleDownInput = true;
            }
            else if (context.canceled)
            {
                IsScaleDownInput = false;
            }
        }


        private void Update()
        {
            CheckJumpInputStayTime();
        }

        private void CheckJumpInputStayTime()
        {
            if (Time.time >= _jumpInputStartTime + inputStayTime)
            {
                JumpInput = false;
            }
        }
    }
}