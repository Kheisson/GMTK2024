using System;
using _Scripts.Infra;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Controls
{
    public class PlayerInputHandler : MonoBehaviour
    {
        [SerializeField] private float inputStayTime;
        [SerializeField] private GameObject inputIndicator;
        private PlayerInput _playerInput;

        private KeyBindingsManager _bindingsManager;
        
        public int NormInputX { get; private set; }
        public bool JumpInput { get; private set; }
        public bool JumpInputStop { get; private set; }
        public bool IsScaleUpInput { get; private set; }
        public bool IsScaleDownInput { get; private set; }
        
        private float _jumpInputStartTime;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
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

        public void OnSwitchPerformed(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _bindingsManager.SwitchPlayer();
            }
        }
        
        public void OnPickUpPerformed(InputAction.CallbackContext context)
        {
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

        public void SetActiveInput(bool isActive)
        {
            if (!isActive)
            {
                NormInputX = 0;
                IsScaleDownInput = false;
                IsScaleUpInput = false;
                JumpInput = false;
            }

            inputIndicator.SetActive(isActive);
            _playerInput.enabled = isActive;
        }

        public void SetBindingMap(string playerMap)
        {
            _playerInput.enabled = true;
            _playerInput.SwitchCurrentActionMap(playerMap);
        }
    }
}