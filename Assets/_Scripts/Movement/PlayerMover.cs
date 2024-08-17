using UnityEngine;

namespace Movement
{
    public class PlayerMover
    {
        private Vector2 _velocity;
        
        private readonly Rigidbody2D _rigidbody2D;
        private int _facingDirection = 1;

        public PlayerMover(Rigidbody2D rigidbody2D)
        {
            _rigidbody2D = rigidbody2D;
        }

        public Vector2 Velocity => _velocity;


        public void SetVelocityX(float velocityX)
        {
            _velocity.Set(velocityX, _rigidbody2D.velocity.y);
            _rigidbody2D.velocity = _velocity;
        }

        public void SetVelocityY(float velocityY)
        {
            _velocity.Set(_rigidbody2D.velocity.x, velocityY);
            _rigidbody2D.velocity = _velocity;
        }
        
        public void AddClampedXVelocity(float amount, float limit, float xInput)
        {
            float addedXVelocity = _rigidbody2D.velocity.x + amount * xInput * Time.fixedDeltaTime;
            float clampedXVelocity = Mathf.Clamp(addedXVelocity, -limit, limit);
            SetVelocityX(clampedXVelocity); 
        }

        public void HandleFlipping(int xInput)
        {
            if (xInput != 0 && xInput != _facingDirection)
            {
                _facingDirection *= -1;
                
                var currentScale = _rigidbody2D.transform.localScale;
                _rigidbody2D.transform.localScale = new Vector3(Mathf.Abs(currentScale.x) * _facingDirection, currentScale.y, currentScale.z);
            }
        }
    }
}