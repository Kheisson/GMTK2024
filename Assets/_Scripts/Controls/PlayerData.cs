using UnityEngine;

namespace Controls
{
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerData", fileName = "New Player Data")]
    public class PlayerData : ScriptableObject
    {
        [field: SerializeField] public float GroundedAcceleration { get; private set; }
        [field: SerializeField] public float InAirAcceleration { get; private set; }
        [field: SerializeField] public float JumpForce { get; private set; }
        [field: SerializeField] public float MaxHorizontalMovementSpeed { get; private set; }
        [field: SerializeField] public float VariableHeightMultiplier { get; set; }
        [field: SerializeField] public float CoyoteTime { get; private set; }
    }
}