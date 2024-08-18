using Animations;
using Movement;
using Scaling;
using UnityEngine;

namespace Controls.StateMachine
{
    public class GroundedState : State
    {
        private readonly PlayerMover _playerMover;
        private bool _isScaleUpInput;
        private bool _isScaleDownInput;
        private bool _isPickupInput;

        public GroundedState(PlayerResources playerResources, FiniteStateMachine stateMachine) : base(playerResources, stateMachine)
        {
            _playerMover = playerResources.PlayerMover;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _isPickupInput = _playerResources.PlayerInputHandler.IsPickUpInput;
            _isScaleUpInput = _playerResources.PlayerInputHandler.IsScaleUpInput;
            _isScaleDownInput = _playerResources.PlayerInputHandler.IsScaleDownInput;
            
            if (_isPickupInput)
            {
                _playerResources.Carrier.ToggleCarry(_playerResources.CollisionDetector.GetScalableGameObject(_playerMover.FacingDirection));
                _playerResources.PlayerInputHandler.IsPickUpInput = false;
            }
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            _playerResources.Animator.SetFloat(AnimationConstants.X_VELOCITY_KEY, Mathf.Abs(_playerMover.Velocity.x));
            _playerResources.Animator.SetBool(AnimationConstants.GROUNDED_KEY, _isGrounded);

            if (_xInput != 0)
            {
                _playerMover.AddClampedXVelocity(_playerResources.PlayerData.GroundedAcceleration,  
                    _playerResources.PlayerData.MaxHorizontalMovementSpeed, _xInput);
                _playerMover.HandleFlipping(_xInput);
                
            }
            else
            {
                _playerMover.SetVelocityX(0);
            }
            
            if (!_isGrounded)
            {
                _stateMachine.ChangeState(new InAirState(_playerResources, _stateMachine, false));
                return;
            }

            if (_isJumpInput)
            {
                _playerMover.SetVelocityY(_playerResources.PlayerData.JumpForce);
                _stateMachine.ChangeState(new InAirState(_playerResources, _stateMachine, true));
                return;
            }
            
            _playerResources.Scaler
                .SetSelectedScalableObject(_playerResources.CollisionDetector.GetScalableObject(_playerMover.FacingDirection));

            if (_isScaleUpInput)
            {
                _playerResources.Scaler.PerformScale(EScaleCommand.ScaleUp, _playerMover.FacingDirection);
            }
            else if (_isScaleDownInput)
            {
                _playerResources.Scaler.PerformScale(EScaleCommand.ScaleDown, _playerMover.FacingDirection);
            } 
        }
    }
}