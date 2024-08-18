using System.Collections.Generic;
using Collisions;
using Scaling;
using Scaling.Scalable;

namespace _Scripts.Scaling
{
    public class ScalerManager
    { 
        private readonly Dictionary<IScalable, HashSet<Scaler>> _scalableToScalers = new Dictionary<IScalable, HashSet<Scaler>>();

        public void UpdateScalableObject(Scaler scaler, (IScalable, ECollisionAxis?) scalableInfo)
        {
            var (newScalable, collisionAxis) = scalableInfo;

            foreach (var kvp in _scalableToScalers)
            {
                kvp.Value.Remove(scaler);
            }

            if (newScalable != null && scaler.CanAffectScalable(newScalable, collisionAxis))
            {
                if (!_scalableToScalers.ContainsKey(newScalable))
                {
                    _scalableToScalers[newScalable] = new HashSet<Scaler>();
                }
                _scalableToScalers[newScalable].Add(scaler);
            }

            ResolveScalableStates();
        }

        private void ResolveScalableStates()
        {
            foreach (var kvp in _scalableToScalers)
            {
                var scalable = kvp.Key;
                var scalers = kvp.Value;

                var shouldSelect = false;

                var scaleXScalers = new List<Scaler>();
                var scaleYScalers = new List<Scaler>();

                foreach (var scaler in scalers)
                {
                    if (scaler.ScaleAbility == EScaleAbility.ScaleX)
                    {
                        scaleXScalers.Add(scaler);
                    }
                    else if (scaler.ScaleAbility == EScaleAbility.ScaleY)
                    {
                        scaleYScalers.Add(scaler);
                    }
                }

                foreach (var scaleYScaler in scaleYScalers)
                {
                    if (scaleYScaler.CollisionAxis == ECollisionAxis.Horizontal)
                    {
                        shouldSelect = true;
                        break;
                    }
                }

                if (!shouldSelect)
                {
                    foreach (var scaleXScaler in scaleXScalers)
                    {
                        if (scaleXScaler.CollisionAxis == ECollisionAxis.Horizontal)
                        {
                            shouldSelect = true;
                        }
                        else if (scaleXScaler.CollisionAxis == ECollisionAxis.Vertical)
                        {
                            if (scaleYScalers.Count > 0)
                            {
                                shouldSelect = true;
                            }
                        }

                        if (shouldSelect)
                        {
                            break;
                        }
                    }
                }

                if (shouldSelect)
                {
                    scalable.Select();
                }
                else
                {
                    scalable.Deselect();
                }
            }
        }
    }
}
