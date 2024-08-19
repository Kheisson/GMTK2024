using _Scripts.Carrier;
using _Scripts.Effects;
using Collisions;
using Movement;
using Player;
using Scaling;
using UnityEngine;

namespace Controls
{
    public class PlayerResources
    {
        public Animator Animator { get; private set; }
        public PlayerInputHandler PlayerInputHandler { get; private set; }
        public PlayerMover PlayerMover { get; private set; }
        public CollisionDetector CollisionDetector { get; private set; }
        public PlayerData PlayerData { get; private set; }
        public Scaler Scaler { get; private set; }
        public Carrier Carrier { get; private set; }
        public EPlayerType PlayerType { get; private set; }
        public ParticleEffectTrigger DustEffectsTrigger { get; set; }

        public PlayerResources(Animator animator, PlayerInputHandler playerInputHandler,
            PlayerMover playerMover, CollisionDetector collisionDetector, PlayerData playerData, Scaler scaler,
            Carrier carrier, EPlayerType playerType, ParticleEffectTrigger particleEffectTrigger)
        {
            Animator = animator;
            PlayerInputHandler = playerInputHandler;
            PlayerMover = playerMover;
            CollisionDetector = collisionDetector;
            PlayerData = playerData;
            Scaler = scaler;
            Carrier = carrier;
            PlayerType = playerType;
            DustEffectsTrigger = particleEffectTrigger;
        }
    }
}