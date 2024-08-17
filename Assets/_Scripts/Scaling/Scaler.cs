using Scaling.Scalable;
using UnityEngine;

namespace Scaling
{
    public class Scaler : MonoBehaviour
    {
        [SerializeField] private EScaleAbility scaleAbility;
        [SerializeField] private float scaleStep = 1.0f;

        private IScalable _scalableObject;
        
        public void PerformScale(EScaleCommand scaleCommand)
        {
            if (_scalableObject == null)
            {
                return;
            }
            
            var direction = scaleAbility switch
            {
                EScaleAbility.ScaleX => Vector3.right,
                EScaleAbility.ScaleY => Vector3.up,
                _ => Vector3.zero,
            };

            var scaleAmount = scaleCommand == EScaleCommand.ScaleUp ? scaleStep : -scaleStep;
            _scalableObject.ScaleObject(direction, scaleAmount);
        }
        
        private void SwitchScaleAbility()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                scaleAbility = scaleAbility == EScaleAbility.ScaleX ? EScaleAbility.ScaleY : EScaleAbility.ScaleX;
            }
        }

        public void SetSelectedScalableObject(IScalable newScalableObject)
        {
            if (_scalableObject != null)
            {
                _scalableObject.Deselect();
            }
            
            _scalableObject = newScalableObject;

            newScalableObject?.Select();
        }
    }
}


