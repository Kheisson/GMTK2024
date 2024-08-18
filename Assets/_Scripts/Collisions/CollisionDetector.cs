using System.Linq;
using Scaling.Scalable;
using UnityEngine;

namespace Collisions
{
    public class CollisionDetector : MonoBehaviour, ISquashable
    {
        [Header("Ground")]
        [SerializeField] private Transform groundChecker;
        [SerializeField] private Vector2 groundCheckArea;
        [SerializeField] private LayerMask groundLayers;

        [Header("Cubes")]
        [SerializeField] private float cubeCheckDistance;
        [SerializeField] private Transform scalableChecker;
        
        [Header("Hazards")]
        [SerializeField] private Transform hazardChecker;
        [SerializeField] private Vector2 hazardCheckArea;
        [SerializeField] private LayerMask hazardLayers;
        
        private Collider2D _collider;
        private bool _isSquashed;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }

        public bool IsGrounded
        {
            get
            {
                (Vector2 pointA, Vector3 pointB) = GetRect(groundChecker, groundCheckArea);

                var hits = Physics2D.OverlapAreaAll(pointA, pointB, groundLayers);
                foreach (var hit in hits)
                {
                    if (hit.transform != transform)
                    {
                        Debug.DrawLine(pointA, pointB, Color.green);
                        return true;
                    }
                }

                Debug.DrawLine(pointA, pointB, Color.red);
                return false;
            }
        }

        public (IScalable, ECollisionAxis?) GetScalableObject(int direction)
        {
            Debug.DrawRay(scalableChecker.position, Vector2.right * (direction * cubeCheckDistance), Color.red);
            var horizontalHits = Physics2D.RaycastAll(scalableChecker.position, Vector2.right * direction, cubeCheckDistance);

            var scalable = horizontalHits.FirstOrDefault(hit => hit.collider.gameObject.name.Contains(nameof(Cube)));

            if (scalable.collider != null)
            {
                var hitDirection = horizontalHits.Contains(scalable) ? ECollisionAxis.Horizontal : ECollisionAxis.Vertical;
                return (scalable.collider.GetComponent<IScalable>(), hitDirection);
            }

            return (null, null);
        }

        public bool IsDetectingHazard
        {
            get
            {
                if (_isSquashed)
                {
                    return true;
                }
                
                (Vector2 pointA, Vector3 pointB) = GetRect(hazardChecker, hazardCheckArea);
                Debug.DrawLine(pointA, pointB, Color.green);

                if (Physics2D.OverlapArea(pointA, pointB, hazardLayers))
                {
                    return true;
                }

                return false;
            }
        }
        
        private (Vector2, Vector2) GetRect(Transform checkerTransform, Vector2 area)
        {
            Vector2 pointA = (Vector2)checkerTransform.position - Vector2.right * area.x;
            Vector2 pointB = (Vector2)checkerTransform.position + Vector2.right * area.x +
                             (Vector2.down) * area.y;
            return (pointA, pointB);
        }
        
        public void Squash()
        {
            _isSquashed = true;
        }
    }
}
