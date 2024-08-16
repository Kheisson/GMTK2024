using Controls.StateMachine;
using UnityEngine;

namespace Controls
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerComponents _playerComponents;
        private FiniteStateMachine _stateMachine;
        private void Awake()
        {
            _playerComponents = new PlayerComponents(
                GetComponent<Rigidbody2D>(), 
                GetComponent<Animator>());
            
            _stateMachine.Initialize(new MoveState(_playerComponents, _stateMachine));
        }
    }

}
