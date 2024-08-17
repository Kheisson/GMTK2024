using UnityEngine;
using Utils;

namespace Controls.StateMachine
{
    public class State : IUpdatable, IFixedUpdatable
    {
        protected PlayerResources _playerResources;
        protected int _xInput;
        protected bool _isGrounded;
        protected bool _isJumpInput;
        protected float _stateEntryTime;
        protected FiniteStateMachine _stateMachine;
            
        public State(PlayerResources playerResources, FiniteStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _playerResources = playerResources;
        }
        
        public virtual void Enter()
        {
            _stateEntryTime = Time.time;
        }

        public virtual void OnUpdate()
        {
            _xInput = _playerResources.PlayerInputHandler.NormInputX;
            _isJumpInput = _playerResources.PlayerInputHandler.JumpInput;
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