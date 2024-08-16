namespace Controls.StateMachine
{
    public class FiniteStateMachine
    {
        public State CurrentState { get; private set; }
        
        public void Initialize(State initialState)
        {
            CurrentState = initialState;
            CurrentState.Enter();
        }
        
        public void ChangeState(State newState)
        {
            CurrentState?.Exit();
            CurrentState = newState;
            CurrentState.Enter();
        }
    }
}