using System;
using _Scripts.Carrier;
using _Scripts.Effects;
using _Scripts.Infra;
using Collisions;
using Controls.StateMachine;
using Movement;
using Player;
using Scaling;
using UnityEngine;

namespace Controls
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerData playerData;
        [SerializeField] private ParticleEffectTrigger dustParticleEffectsTrigger;
        [field: SerializeField] public EPlayerType PlayerType { get; private set; }

        private FiniteStateMachine _stateMachine;
        private PlayerResources _playerResources;

        private void Awake()
        { 
            _playerResources = new PlayerResources(
                animator: GetComponentInChildren<Animator>(), 
                playerInputHandler: GetComponent<PlayerInputHandler>(),
                playerMover: new PlayerMover(GetComponent<Rigidbody2D>()),
                collisionDetector: GetComponent<CollisionDetector>(),
                playerData: playerData,
                scaler: GetComponent<Scaler>(),
                carrier: GetComponent<Carrier>(),
                playerType: PlayerType,
                particleEffectTrigger: dustParticleEffectsTrigger);

            _stateMachine = new FiniteStateMachine();
            _stateMachine.Initialize(new GroundedState(_playerResources, _stateMachine));
        }

        private void Start()
        {
            SetupBindings(ServiceLocator.GetService<KeyBindingsManager>().IsSharedBindings);
        }

        private void OnEnable()
        {
            var bindingsManager = ServiceLocator.GetService<KeyBindingsManager>();
            OnCurrentActiveUserChanged(bindingsManager.CurrentActivePlayer);
            bindingsManager.OnActivePlayerChanged += OnCurrentActiveUserChanged;
            bindingsManager.OnBindingsChanged += SetupBindings;
        }

        private void SetupBindings(bool isShared)
        {
            if (!isShared)
            {
                _playerResources.PlayerInputHandler.SetBindingMap(PlayerType == EPlayerType.X
                    ? InputConstants.PLAYER_X_MAP
                    : InputConstants.PLAYER_Y_MAP);
                _playerResources.PlayerInputHandler.SetActiveInput(false);
            }
            else
            {
                var currentActivePlayer = ServiceLocator.GetService<KeyBindingsManager>().CurrentActivePlayer;
                _playerResources.PlayerInputHandler.SetBindingMap(InputConstants.SINGLEPLAYER_MAP);
                _playerResources.PlayerInputHandler.SetActiveInput(PlayerType == currentActivePlayer);
            }
        }

        private void OnDisable()
        {
            var bindingsManager = ServiceLocator.GetService<KeyBindingsManager>();
            bindingsManager.OnActivePlayerChanged -= OnCurrentActiveUserChanged;
            bindingsManager.OnBindingsChanged -= SetupBindings;
        }

        private void OnCurrentActiveUserChanged(EPlayerType playerType)
        {
            _playerResources.PlayerInputHandler.SetActiveInput(playerType == PlayerType);
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
