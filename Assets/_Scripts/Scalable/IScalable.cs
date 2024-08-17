using System;
using UnityEngine;

namespace Scalable
{
    public interface IScalable
    {
        bool CanScale { get; }
        void ScaleObject(Vector3 direction, float scaleAmount);
        event Action OnScaleSuccess;
        event Action OnScaleFailure;
    }
}