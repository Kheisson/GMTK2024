using Scaling;
using Scaling.Scalable;
using UnityEngine;

namespace Player
{
    public class Player : MonoBehaviour, IPlayer
    {
        [SerializeField] private float scaleStep = 1.0f;
        public EScaleAbility scaleAbility;
        private IScalable _scalableObject;

        private void Update()
        {
            Move();
            InteractWithObject();
#if UNITY_EDITOR
            SwitchScaleAbility();
#endif
        }

        public void Move()
        {
            Vector3 moveDirection = Vector3.zero;

            if (Input.GetKey(KeyCode.W)) moveDirection += Vector3.up;
            if (Input.GetKey(KeyCode.S)) moveDirection += Vector3.down;
            if (Input.GetKey(KeyCode.A)) moveDirection += Vector3.left;
            if (Input.GetKey(KeyCode.D)) moveDirection += Vector3.right;

            transform.position += moveDirection * Time.deltaTime;
        }

        public void InteractWithObject()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                PerformScale(scaleAbility, EScaleCommand.ScaleUp);
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                PerformScale(scaleAbility, EScaleCommand.ScaleDown);
            }
        }

        private void PerformScale(EScaleAbility ability, EScaleCommand scaleCommand)
        {
            if (_scalableObject == null) return;

            var direction = ability switch
            {
                EScaleAbility.ScaleX => Vector3.right,
                EScaleAbility.ScaleY => Vector3.up,
                _ => Vector3.zero,
            };

            var scaleAmount = scaleCommand == EScaleCommand.ScaleUp ? scaleStep : -scaleStep;
            _scalableObject.ScaleObject(direction, scaleAmount);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.name.Contains(nameof(Cube)))
            {
                Debug.Log("Collision with cube");
                _scalableObject = other.gameObject.GetComponent<IScalable>();
                
                if (_scalableObject != null)
                {
                    Debug.Log("Cube is scalable");
                }
            }
        }

        private void OnCollisionExit2D(Collision2D other)
        {
            if (other.gameObject.name.Contains(nameof(Cube)))
            {
                Debug.Log("Exit collision with cube");
                if (_scalableObject != null)
                {
                    Debug.Log("Cube is no longer scalable");
                }
                _scalableObject = null;
            }
        }
        
        #if UNITY_EDITOR
        
        private void SwitchScaleAbility()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                scaleAbility = scaleAbility == EScaleAbility.ScaleX ? EScaleAbility.ScaleY : EScaleAbility.ScaleX;
            }
        }
        
        #endif
    }
}
