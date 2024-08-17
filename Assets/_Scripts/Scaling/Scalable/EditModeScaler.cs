using System;
using UnityEngine;

namespace Scaling.Scalable
{
    [ExecuteInEditMode]
    public class EditModeScaler : MonoBehaviour
    {
        [SerializeField, Range(1, 10)] private int scaleX;
        [SerializeField, Range(1, 10)] private int scaleY;
        private BoxCollider2D _collider;
        private SpriteRenderer _renderer;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider2D>();
            _renderer = GetComponent<SpriteRenderer>();
        }

        private void OnValidate()
        {
            if (_collider == null || _renderer == null)
            {
                return;
            }
            _collider.size = new Vector2(scaleX, scaleY);
            _renderer.size = new Vector2(scaleX, scaleY);
        }
    }
}