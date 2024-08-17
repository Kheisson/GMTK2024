using Movement;

namespace Controls.StateMachine
{
    public class InAirState : State
    {
        private readonly PlayerMover _playerMover;
        private bool _isJumping;
        private bool _isJumpInputStop;

        public InAirState(PlayerResources playerResources, FiniteStateMachine stateMachine, bool isJumping) : base(playerResources,
            stateMachine)
        {
            _playerMover = playerResources.PlayerMover;
            _isJumping = isJumping;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _isJumpInputStop = _playerResources.PlayerInputHandler.JumpInputStop;

            //CheckJumpMultiplier();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (_isGrounded)
            {
                _stateMachine.ChangeState(new GroundedState(_playerResources, _stateMachine));
                return;
            }
            
            _playerMover.HandleFlipping(_xInput);
            
            _playerMover.AddClampedXVelocity(_playerResources.PlayerData.InAirAcceleration,
                _playerResources.PlayerData.MaxHorizontalMovementSpeed, _xInput);
        }
    }
}