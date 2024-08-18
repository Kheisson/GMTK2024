using _Scripts.Infra;
using _Scripts.Scaling;
using Collisions;
using Scaling.Scalable;
using UnityEngine;

namespace Scaling
{
    public class Scaler : MonoBehaviour
    {
        [SerializeField] private EScaleAbility scaleAbility;
        [SerializeField] private float scaleStep = 1.0f;

        private ScalerManager _scalerManager;
        private IScalable _scalableObject;
        
        public ECollisionAxis? CollisionAxis { get; private set; }
        public EScaleAbility ScaleAbility => scaleAbility;

        private void Start()
        {
            _scalerManager = ServiceLocator.GetService<ScalerManager>();
        }

        public void PerformScale(EScaleCommand scaleCommand, int facingDirection)
        {
            if (_scalableObject == null) return;

            var direction = scaleAbility switch
            {
                EScaleAbility.ScaleX => facingDirection > 0 ? Vector3.right : Vector3.left,
                EScaleAbility.ScaleY => Vector3.up,
                _ => Vector3.zero,
            };

            var scaleAmount = scaleCommand == EScaleCommand.ScaleUp ? scaleStep : -scaleStep;
            _scalableObject.ScaleObject(direction, scaleAmount);
        }

        public bool CanAffectScalable(IScalable scalable, ECollisionAxis? collisionAxis)
        {
            if (scalable == null) return false;

            if (collisionAxis == ECollisionAxis.Vertical && scaleAbility == EScaleAbility.ScaleY)
            {
                return false;
            }

            if (collisionAxis == ECollisionAxis.Horizontal && scaleAbility == EScaleAbility.ScaleY)
            {
                return true; 
            }

            return true; 
        }

        public void UpdateScalableObject((IScalable, ECollisionAxis?) scalableInfo)
        {
            if (scalableInfo.Item1 != _scalableObject || scalableInfo.Item2 != CollisionAxis)
            {
                _scalableObject = scalableInfo.Item1;
                CollisionAxis = scalableInfo.Item2;
                _scalerManager.UpdateScalableObject(this, scalableInfo);
            }
        }
    }
}
