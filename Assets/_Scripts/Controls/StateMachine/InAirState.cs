namespace Controls.StateMachine
{
    public class InAirState : State
    {
        public InAirState(PlayerResources playerResources, FiniteStateMachine stateMachine) : base(playerResources,
            stateMachine)
        {
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (_isGrounded)
            {
                _stateMachine.ChangeState(new GroundedState(_playerResources, _stateMachine));
            }
        }

    }
}