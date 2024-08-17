using System;
using UnityEngine;

namespace Controls
{
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerData", fileName = "New Player Data")]
    public class PlayerData : ScriptableObject
    {
        [field: SerializeField] public float MovementSpeed { get; private set; }
    }
}