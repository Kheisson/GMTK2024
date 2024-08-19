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

            if (bindingsManager.IsSharedBindings)
            {
                SetCurrentActiveUser(bindingsManager.CurrentActivePlayer);
                bindingsManager.OnActivePlayerChanged += SetCurrentActiveUser;
                bindingsManager.OnBindingsChanged += SetupBindings;
            }
        }

        private void SetupBindings(bool isShared)
        {
            if (isShared)
            {
                var currentActivePlayer = ServiceLocator.GetService<KeyBindingsManager>().CurrentActivePlayer;
                _playerResources.PlayerInputHandler.SetBindingMap(InputConstants.SINGLEPLAYER_MAP);
                _playerResources.PlayerInputHandler.SetActiveInput(PlayerType == currentActivePlayer);
            }
            else
            {
                _playerResources.PlayerInputHandler.SetBindingMap(PlayerType == EPlayerType.X
                    ? InputConstants.PLAYER_X_MAP
                    : InputConstants.PLAYER_Y_MAP);
                _playerResources.PlayerInputHandler.SetActiveInput(false);
            }
        }

        private void OnDisable()
        {
            var bindingsManager = ServiceLocator.GetService<KeyBindingsManager>();
            bindingsManager.OnActivePlayerChanged -= SetCurrentActiveUser;
            bindingsManager.OnBindingsChanged -= SetupBindings;
        }

        private void SetCurrentActiveUser(EPlayerType playerType)
        {
            Debug.Log("current active user changed to: " + playerType + "setting input to: " + (playerType == PlayerType));
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
