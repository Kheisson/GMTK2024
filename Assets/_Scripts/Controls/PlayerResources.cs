using Collisions;
using Movement;
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

        public PlayerResources(Animator animator, PlayerInputHandler playerInputHandler,
            PlayerMover playerMover, CollisionDetector collisionDetector, PlayerData playerData, Scaler scaler)
        {
            Animator = animator;
            PlayerInputHandler = playerInputHandler;
            PlayerMover = playerMover;
            CollisionDetector = collisionDetector;
            PlayerData = playerData;
            Scaler = scaler;
        }
    }
}