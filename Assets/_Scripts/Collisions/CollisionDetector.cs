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
                foreach (Collider2D hit in hits)
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

        public IScalable GetScalableObject(int direction)
        {
            var hits = Physics2D.RaycastAll(scalableChecker.position, Vector2.right * direction, cubeCheckDistance);
            var scalable = hits.FirstOrDefault(hit => hit.collider.gameObject.name.Contains(nameof(Cube)));

            return scalable? scalable.collider.GetComponent<IScalable>() : null;
        }
        
        public GameObject GetScalableGameObject(int direction)
        {
            var hits = Physics2D.RaycastAll(scalableChecker.position, Vector2.right * direction, cubeCheckDistance);
            var scalable = hits.FirstOrDefault(hit => hit.collider.gameObject.name.Contains(nameof(Cube)));

            return scalable.collider?.gameObject;
        }
    }
}