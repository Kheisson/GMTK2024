using _Scripts.Carrier;
using Collisions;
using Controls.StateMachine;
using Movement;
using Scaling;
using UnityEngine;

namespace Controls
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;

        private FiniteStateMachine _stateMachine;
        
        private void Awake()
        {
            PlayerResources playerResources = new PlayerResources(
                animator: GetComponent<Animator>(), 
                playerInputHandler: GetComponent<PlayerInputHandler>(),
                playerMover: new PlayerMover(GetComponent<Rigidbody2D>()),
                collisionDetector: GetComponent<CollisionDetector>(),
                playerData: playerData,
                scaler: GetComponent<Scaler>(),
                carrier: GetComponent<Carrier>());

            _stateMachine = new FiniteStateMachine();
            _stateMachine.Initialize(new GroundedState(playerResources, _stateMachine));
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
