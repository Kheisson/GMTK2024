using Movement;

namespace Controls.StateMachine
{
    public class GroundedState : State
    {
        private readonly PlayerInputHandler _playerInputHandler;
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
            _playerMover.Move(_xInput);
            _playerMover.HandleFlipping(_xInput);

            if (!_isGrounded)
            {
                _stateMachine.ChangeState(new InAirState(_playerResources, _stateMachine));
            }
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}