using Animations;
using Movement;
using UnityEngine;

namespace Controls.StateMachine
{
    public class InAirState : State
    {
        private readonly PlayerMover _playerMover;
        private bool _isJumping;
        private bool _isJumpInputStop;
        private bool _isCoyoteTimeActive;

        public InAirState(PlayerResources playerResources, FiniteStateMachine stateMachine, bool isJumping) : base(playerResources,
            stateMachine)
        {
            _playerMover = playerResources.PlayerMover;
            _isJumping = isJumping;
            _isCoyoteTimeActive = !isJumping;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _isJumpInputStop = _playerResources.PlayerInputHandler.JumpInputStop;
            _playerResources.Animator.SetBool(AnimationConstants.GROUNDED_KEY, _isGrounded);
            _playerResources.Animator.SetFloat(AnimationConstants.Y_VELOCITY_KEY, _playerMover.Velocity.y);

            CheckCoyoteTime();
            CheckJumpMultiplier();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (_isGrounded)
            {
                _stateMachine.ChangeState(new GroundedState(_playerResources, _stateMachine));
                return;
            }

            if (_isCoyoteTimeActive && _isJumpInput)
            {
                Jump();
            }
            
            _playerMover.HandleFlipping(_xInput);
            
            _playerMover.AddClampedXVelocity(_playerResources.PlayerData.InAirAcceleration,
                _playerResources.PlayerData.MaxHorizontalMovementSpeed, _xInput);
        }

        private void Jump()
        {
            _playerMover.SetVelocityY(_playerResources.PlayerData.JumpForce);
            _isJumping = true;
            _isCoyoteTimeActive = false;
        }

        private void CheckJumpMultiplier()
        {
            if (!_isJumping)
            {
                return;
            }
            
            if(_playerMover.Velocity.y <= 0f)
            {
                _isJumping = false;
            }
            else if (_isJumpInputStop)
            {
                _playerMover.SetVelocityY(_playerMover.Velocity.y * _playerResources.PlayerData.VariableHeightMultiplier);
                _isJumping = false;
            }
        }
        
        private void CheckCoyoteTime()
        {
            if (_isCoyoteTimeActive && Time.time > _stateEntryTime + _playerResources.PlayerData.CoyoteTime)
            {
                _isCoyoteTimeActive = false;
            }
        }
    }
}