using Collisions;
using Controls.StateMachine;
using Movement;
using UnityEngine;

namespace Controls
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;

        private PlayerResources _playerResources;
        private FiniteStateMachine _stateMachine;
        private PlayerInputHandler _inputHandler;
        
        private void Awake()
        {
            var rigidbody = GetComponent<Rigidbody2D>();
            _inputHandler = GetComponent<PlayerInputHandler>();
            
            _playerResources = new PlayerResources(
                rigidbody2D: rigidbody,
                animator: GetComponent<Animator>(), 
                playerInputHandler: _inputHandler,
                playerMover: new PlayerMover(GetComponent<Rigidbody2D>()),
                collisionDetector: GetComponent<CollisionDetector>(),
                playerData: playerData);

            _stateMachine = new FiniteStateMachine();
            _stateMachine.Initialize(new GroundedState(_playerResources, _stateMachine));
        }

        private void Update()
        {
            _stateMachine.OnUpdate();
        }

        private void FixedUpdate()
        {
            _stateMachine.OnFixedUpdate();
        }
    }

}
