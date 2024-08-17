using Utils;

namespace Controls.StateMachine
{
    public class State : IUpdatable, IFixedUpdatable
    {
        protected PlayerResources _playerResources;
        protected int _xInput;
        protected bool _isGrounded;
        protected FiniteStateMachine _stateMachine;
            
        public State(PlayerResources playerResources, FiniteStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _playerResources = playerResources;
        }
        
        public virtual void Enter()
        {
            
        }

        public virtual void OnUpdate()
        {
            _xInput = _playerResources.PlayerInputHandler.NormInputX;
        }

        public virtual void OnFixedUpdate()
        {
            _isGrounded = _playerResources.CollisionDetector.IsGrounded;
        }

        public virtual void Exit()
        {
            
        }
    }
}