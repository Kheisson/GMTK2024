using _Scripts.Carrier;
using _Scripts.Effects;
using Animations;
using Collisions;
using Controls.StateMachine;
using Movement;
using Player;
using Scaling;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Controls
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private GameObject activePlayerIndicator;
        [SerializeField] private InputActionAsset actions;
        [SerializeField] private PlayerData playerData;
        [SerializeField] private ParticleEffectTrigger dustParticleEffectsTrigger;

        [field: SerializeField] public EPlayerType PlayerType { get; private set; }

        private InputActionMap _currentActionMap;
        private PlayerInput _playerInput;
        private FiniteStateMachine _stateMachine;
        private PlayerResources _playerResources;

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();

            _playerResources = new PlayerResources(
                animator: GetComponentInChildren<Animator>(),
                playerInputHandler: GetComponent<PlayerInputHandler>(),
                playerMover: new PlayerMover(GetComponent<Rigidbody2D>()),
                collisionDetector: GetComponent<CollisionDetector>(),
                playerData: playerData,
                scaler: GetComponent<Scaler>(),
                carrier: GetComponent<Carrier>(),
                playerType: PlayerType,
                particleEffectTrigger: dustParticleEffectsTrigger
            );
            
            _stateMachine = new FiniteStateMachine();
            _stateMachine.Initialize(new GroundedState(_playerResources, _stateMachine));
        }

        public void InitializeInput(string actionMapName)
        {
            _currentActionMap?.Disable();
            Debug.Log($"Initializing Input for {gameObject.name} with ActionMap: {actionMapName}");
            _currentActionMap = _playerInput.actions.FindActionMap(actionMapName);
            _currentActionMap.Enable();
        }

        public PlayerController SetAsCurrentPlayer()
        {
            activePlayerIndicator.SetActive(true);
            EnablePlayerInput();
            return this;
        }

        private void EnablePlayerInput()
        {
            Debug.Log($"Enabling PlayerInput for {gameObject.name}");
            _playerInput.enabled = true;
        }

        private void DisablePlayerInput()
        {
            Debug.Log($"Disabling PlayerInput for {gameObject.name}");
            _playerInput.enabled = false;
            _playerResources.PlayerMover.SetVelocityX(0);
            _playerResources.Animator.SetFloat(AnimationConstants.X_VELOCITY_KEY, Mathf.Abs(0));
            _playerResources.Animator.SetBool(AnimationConstants.GROUNDED_KEY, true);

        }

        public void OnSetInactive()
        {
            activePlayerIndicator.SetActive(false);
            DisablePlayerInput();
        }


        private void Update()
        {
            if (_playerInput.enabled)
            {
                _stateMachine.OnUpdate();
            }
        }

        private void FixedUpdate()
        {
            if (_playerInput.enabled)
            {
                _stateMachine.OnFixedUpdate();
            }
        }
    }
}
