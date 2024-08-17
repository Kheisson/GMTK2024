using Collisions;
using Movement;
using UnityEngine;

namespace Controls
{
    public class PlayerResources
    {
        public Rigidbody2D Rigidbody2D { get; private set; }
        public Animator Animator { get; private set; }
        public PlayerInputHandler PlayerInputHandler { get; private set; }
        public PlayerMover PlayerMover { get; private set; }
        public CollisionDetector CollisionDetector { get; set; }

        public PlayerResources(Rigidbody2D rigidbody2D, Animator animator, PlayerInputHandler playerInputHandler,
            PlayerMover playerMover, CollisionDetector collisionDetector)
        {
            Rigidbody2D = rigidbody2D;
            Animator = animator;
            PlayerInputHandler = playerInputHandler;
            PlayerMover = playerMover;
            CollisionDetector = collisionDetector;
        }
    }
}