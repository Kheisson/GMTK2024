using Movement;

namespace Controls.StateMachine
{
    public class GroundedState : State
    {
        private readonly PlayerMover _playerMover;

        public GroundedState(PlayerResources playerResources, FiniteStateMachine stateMachine) : base(playerResources, stateMachine)
        {
            _playerMover = playerResources.PlayerMover;
        }

        public override void Enter()
        {
            base.Enter();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
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
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}