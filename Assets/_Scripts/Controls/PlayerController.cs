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
            _inputHandler = new PlayerInputHandler();
            
            _playerResources = new PlayerResources(
                rigidbody2D: rigidbody,
                animator: GetComponent<Animator>(), 
                playerInputHandler: _inputHandler,
                playerMover: new PlayerMover(rigidbody, playerData.MovementSpeed),
                collisionDetector: GetComponent<CollisionDetector>());

            _stateMachine = new FiniteStateMachine();
            _stateMachine.Initialize(new GroundedState(_playerResources, _stateMachine));
        }

        private void OnEnable()
        {
            _inputHandler.Enable();
        }

        private void OnDisable()
        {
            _inputHandler.Disable();
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
