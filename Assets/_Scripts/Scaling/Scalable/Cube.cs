using System;
using _Scripts.Audio;
using _Scripts.Infra;
using Collisions;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Scaling.Scalable
{
    [RequireComponent(typeof(Collider2D), typeof(Renderer))]
    public class Cube : MonoBehaviour, IScalable
    {
        private const float SCALING_DURATION = 0.25f;
        
        [SerializeField, ColorUsage(true, true)] private Color activeColor;
        [SerializeField, ColorUsage(true, true)] private Color inactiveColor;
        [SerializeField] private float squashingDistance = 0.2f;
        [SerializeField] private float minScale = 1.0f;
        [SerializeField] private LayerMask collisionLayer;
        [SerializeField] private LayerMask squashingLayers;
        [SerializeField] private float squashOffset = 0.1f;
        [SerializeField] private float scaleThreshold = 0.1f;


        private readonly Collider2D[] _collisionResults = new Collider2D[10];
        private BoxCollider2D _collider2D;
        private OutlineFx.OutlineFx _outlineFx;
        private SpriteRenderer _spriteRenderer;
        public event Action OnScaleSuccess;
        public event Action OnScaleFailure;

        public bool CanScale { get; private set; } = true;
        
        private void Awake()
        {
            _collider2D = GetComponent<BoxCollider2D>();
            _outlineFx = GetComponent<OutlineFx.OutlineFx>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void ScaleObject(Vector3 direction, float scaleAmount)
        {
            if (!CanScale)
            {
                return;
            }

            var newScale = _collider2D.size;
            var scaleDelta = Vector3.zero;
            
            if (direction == Vector3.right)
            {
                newScale.x = Mathf.Max(newScale.x + scaleAmount, minScale);
                scaleDelta = new Vector3((scaleAmount)/ 2f, 0, 0);
            }
            else if (direction == Vector3.left)
            {
                newScale.x = Mathf.Max(newScale.x + scaleAmount, minScale);
                scaleDelta = new Vector3(-scaleAmount / 2f, 0, 0);
            }
            else if (direction == Vector3.up)
            {
                newScale.y = Mathf.Max(newScale.y + scaleAmount, minScale);
                scaleDelta = new Vector3(0, scaleAmount / 2f, 0);
            }

            if (ShouldScaleFail(direction, scaleAmount))
            {
                OnScaleFailure?.Invoke();
                
                return;
            }
            
            var audioName = scaleAmount > 0 ? AudioConstants.SCALE_UP_KEY : AudioConstants.SCALE_DOWN_KEY;
            ServiceLocator.GetService<AudioManager>().PlaySfx(audioName);

            UniTask.Void(async () =>
            {
                var newPosition = transform.position + scaleDelta;
                _collider2D.size = new Vector2(Mathf.RoundToInt(_collider2D.size.x), Mathf.RoundToInt(_collider2D.size.y));
                
                var token = this.GetCancellationTokenOnDestroy();
                CanScale = false;
                
                await UniTask.WhenAll(

                    DOTween.To(() => (Vector3)_collider2D.size, value => 
                    {
                        _collider2D.size = value;
                    }, (Vector3)newScale , SCALING_DURATION).SetEase(Ease.InSine).WithCancellation(token),
                    
                    DOTween.To(() => (Vector3)_spriteRenderer.size, value => 
                    {
                        _spriteRenderer.size = value;
                    }, (Vector3)newScale, SCALING_DURATION).SetEase(Ease.InSine).WithCancellation(token),
                    
                    transform.DOMove(newPosition, SCALING_DURATION).SetEase(Ease.InSine).WithCancellation(token)
                );

                _collider2D.size = new Vector2(Mathf.RoundToInt(_collider2D.size.x) - scaleThreshold, Mathf.RoundToInt(_collider2D.size.y) - scaleThreshold);

                TrySquashInDirection(direction);
                
                CanScale = true;
                OnScaleSuccess?.Invoke();
            });
        }

        private void TrySquashInDirection(Vector3 direction)
        {
            var hits = Physics2D.BoxCastAll(transform.position, _collider2D.size, 0, direction, squashingDistance, squashingLayers);

            if (hits == null)
            {
                return;
            }
            
            AttemptToSquash(direction, hits);
            
        }

        private void AttemptToSquash(Vector3 direction, RaycastHit2D[] hits)
        {
            var bounds = _collider2D.bounds;
            var offsetBounds = new Bounds();
            var offsetMin = bounds.min + new Vector3(squashOffset, squashOffset);
            var offsetMax = bounds.max - new Vector3(squashOffset, squashOffset);
            
            offsetBounds.SetMinMax(offsetMin, offsetMax);

            foreach (var hit in hits)
            {
                if (hit.collider.TryGetComponent<ISquashable>(out var squashable))
                {
                    if (offsetBounds.Contains(hit.transform.position))
                    {
                        squashable.Squash();
                    }
                }
            }
        }

        public void Deselect()
        {
            ActivateOutline(false);
        }

        public void Select()
        {
            ActivateOutline(true);
        }
        
        private bool ShouldScaleFail(Vector3 direction, float scaleAmount)
        {
            switch (scaleAmount)
            {
                case > 0 when WillCauseCollision(direction, scaleAmount):
                case < 0 when (direction == Vector3.right && _collider2D.size.x <= minScale) || 
                              (direction == Vector3.left && _collider2D.size.x <= minScale) ||
                              (direction == Vector3.up && _collider2D.size.y <= minScale):
                    return true;
                default:
                    return false; 
            }
        }

        private bool WillCauseCollision(Vector3 direction, float scaleAmount)
        {
            var originalSize = _collider2D.bounds.size;
            var scaledSize = originalSize * 0.9f;

            var extendedSize = new Vector3(
                direction.x != 0 ? scaledSize.x + 1 : scaledSize.x,
                direction.y != 0 ? scaledSize.y + 1 : scaledSize.y,
                originalSize.z
            );

            var boxCenter = _collider2D.bounds.center + direction * scaleAmount / 2;

            var hitCount = Physics2D.OverlapBoxNonAlloc(boxCenter, extendedSize, 0f, _collisionResults, collisionLayer);

            debugBoxCenter = boxCenter;
            debugBoxSize = extendedSize;
            shouldDrawDebugBox = true;

            for (var i = 0; i < hitCount; i++)
            {
                if (_collisionResults[i].gameObject != gameObject)
                {
                    return true;
                }
            }

            return false;
        }
        
        private Vector3 debugBoxCenter;
        private Vector3 debugBoxSize;
        private bool shouldDrawDebugBox = false;
        
        void OnDrawGizmos()
        {
            if (shouldDrawDebugBox)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(debugBoxCenter, debugBoxSize);
            }
        }
        
        private void ActivateOutline(bool activate)
        {
            if (_outlineFx != null)
            {
                _outlineFx.enabled = activate;
                _outlineFx.Color = activate ? activeColor : inactiveColor;
            }
        }
    }
}
