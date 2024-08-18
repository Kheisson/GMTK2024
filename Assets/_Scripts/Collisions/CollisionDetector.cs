using System.Linq;
using Scaling.Scalable;
using UnityEngine;

namespace Collisions
{
    public class CollisionDetector : MonoBehaviour
    {
        [SerializeField] private Transform groundChecker;
        [SerializeField] private Transform scalableChecker;
        [SerializeField] private Vector2 groundCheckArea;
        [SerializeField] private float cubeCheckDistance;

        public bool IsGrounded
        {
            get
            {
                Vector2 pointA = (Vector2)groundChecker.position - Vector2.right * groundCheckArea.x;
                Vector2 pointB = (Vector2)groundChecker.position + Vector2.right * groundCheckArea.x +
                                 (Vector2.down) * groundCheckArea.y;

                var hits = Physics2D.OverlapAreaAll(pointA, pointB);
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
            const float downwardCubeCheckDistance = 0.8f;
            var horizontalHits = Physics2D.RaycastAll(scalableChecker.position, Vector2.right * direction, cubeCheckDistance);
            var downwardHits = Physics2D.RaycastAll(scalableChecker.position, Vector2.down, downwardCubeCheckDistance);
            var hits = horizontalHits.Concat(downwardHits);

            var scalable = hits.FirstOrDefault(hit => hit.collider.gameObject.name.Contains(nameof(Cube)));

            if (scalable.collider != null)
            {
                var hitDirection = horizontalHits.Contains(scalable) ? ECollisionAxis.Horizontal : ECollisionAxis.Vertical;
                return (scalable.collider.GetComponent<IScalable>(), hitDirection);
            }

            return (null, null);
        }
    }
}
