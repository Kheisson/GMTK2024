using UnityEngine;

namespace Collisions
{
    public class CollisionDetector : MonoBehaviour
    {
        [SerializeField] private Transform groundCheck;
        [SerializeField] private Vector2 groundCheckArea;

        public bool IsGrounded
        {
            get
            {
                Vector2 pointA = (Vector2)groundCheck.position - Vector2.right * groundCheckArea.x;
                Vector2 pointB = (Vector2)groundCheck.position + Vector2.right * groundCheckArea.x +
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
    }
}