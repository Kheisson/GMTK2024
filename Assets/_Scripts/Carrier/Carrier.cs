using UnityEngine;

namespace _Scripts.Carrier
{
    public class Carrier : MonoBehaviour
    {
        [SerializeField] private float liftHeight = 1.0f;
        [SerializeField] private float forwardOffset = 0.5f;

        private GameObject _heldObject;
        private Collider2D _heldObjectCollider;
        private Collider2D _playerCollider;

        private void Awake()
        {
            _playerCollider = GetComponent<Collider2D>();
        }

        public void ToggleCarry(GameObject obj)
        {
            if (_heldObject != null)
            {
                Drop();
            }
            else
            {
                PickUp(obj);
            }
        }

        private void PickUp(GameObject obj)
        {
            if (_heldObject != null)
            {
                return;
            }

            _heldObject = obj;
            _heldObjectCollider = _heldObject?.GetComponent<Collider2D>();
            
            if (_heldObjectCollider == null)
            {
                return;
            }
            
            _heldObjectCollider.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            Physics2D.IgnoreCollision(_playerCollider, _heldObjectCollider, true);
            PositionObject();
        }

        private void Drop()
        {
            if (!_heldObject)
            {
                return;
            }

            _heldObjectCollider.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
            Physics2D.IgnoreCollision(_playerCollider, _heldObjectCollider, false); 
            _heldObject = null;
        }

        public bool IsHoldingObject()
        {
            return _heldObject != null;
        }

        private void PositionObject()
        {
            if (_heldObject == null) return;
            
            var forwardDirection = transform.localScale.x > 0 ? Vector3.right : Vector3.left;
            var forwardPosition = transform.position + forwardDirection * forwardOffset;
            var liftedPosition = new Vector3(forwardPosition.x, transform.position.y + liftHeight, forwardPosition.z);
            _heldObject.transform.position = liftedPosition;
        }

        private void Update()
        {
            if (_heldObject != null)
            {
                PositionObject();
            }
        }
    }
}
