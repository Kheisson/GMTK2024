using Movement;
using Scaling;

namespace Controls.StateMachine
{
    public class GroundedState : State
    {
        private readonly PlayerMover _playerMover;
        private bool _isScaleUpInput;
        private bool _isScaleDownInput;

        public GroundedState(PlayerResources playerResources, FiniteStateMachine stateMachine) : base(playerResources, stateMachine)
        {
            _playerMover = playerResources.PlayerMover;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _isScaleUpInput = _playerResources.PlayerInputHandler.IsScaleUpInput;
            _isScaleDownInput = _playerResources.PlayerInputHandler.IsScaleDownInput;
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

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
                _playerResources.Scaler.PerformScale(EScaleCommand.ScaleUp);
            }
            else if (_isScaleDownInput)
            {
                _playerResources.Scaler.PerformScale(EScaleCommand.ScaleDown);
            }
        }
    }
}