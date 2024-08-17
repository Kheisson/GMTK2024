using UnityEngine;

namespace Movement
{
    public class PlayerMover
    {
        private readonly Rigidbody2D _rigidbody2D;
        private readonly float _moveSpeed;
        public int _facingDirection = 1;

        public PlayerMover(Rigidbody2D rigidbody2D, float moveSpeed)
        {
            _rigidbody2D = rigidbody2D;
            _moveSpeed = moveSpeed;
        }

        public void Move(int xInput)
        {
           _rigidbody2D.MovePosition(_rigidbody2D.position + Vector2.right * (xInput * _moveSpeed * Time.fixedDeltaTime));
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