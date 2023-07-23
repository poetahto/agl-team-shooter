using UnityEngine;

public abstract class MovementStrategy : ScriptableObject
{
    public struct MovementState
    {
        public Vector3 InputDirection;
        public Vector3 CurrentVelocity;
        public bool IsGrounded;
        public float CurrentSpeed;
        public float TimeSpentOnGround;
        public float MaxSpeed;
    }

    public abstract Vector3 CalculateVelocity(ref MovementState state);
}
