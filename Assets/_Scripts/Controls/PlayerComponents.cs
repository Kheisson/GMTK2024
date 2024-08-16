using UnityEngine;

namespace Controls
{
    public class PlayerComponents
    {
        public Rigidbody2D Rigidbody2D { get; private set; }
        public Animator Animator { get; private set; }

        public PlayerComponents(Rigidbody2D rigidbody2D, Animator animator)
        {
            Rigidbody2D = rigidbody2D;
            Animator = animator;
        }
    }
}