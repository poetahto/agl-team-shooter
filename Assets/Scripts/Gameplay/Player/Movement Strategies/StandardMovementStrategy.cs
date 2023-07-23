using UnityEngine;

[CreateAssetMenu]
public class StandardMovementStrategy : MovementStrategy
{
    [Header("Settings")]
    [SerializeField] public float groundedAcceleration = 0.125f;
    [SerializeField] public float groundedDeceleration = 0.125f;
    [SerializeField] public float airborneAcceleration = 0.3f;
    [SerializeField] public float airborneDeceleration = 0.3f;

    public override Vector3 CalculateVelocity(ref MovementState state)
    {
        Vector3 targetVelocity = state.InputDirection * state.MaxSpeed;

        float currentGroundedAcceleration = IsAccelerating(targetVelocity)
            ? CalculateAcceleration(groundedAcceleration, ref state)
            : CalculateAcceleration(groundedDeceleration, ref state);

        float currentAirborneAcceleration = IsAccelerating(targetVelocity)
            ? CalculateAcceleration(airborneAcceleration, ref state)
            : CalculateAcceleration(airborneDeceleration, ref state);

        float acceleration = state.IsGrounded ? currentGroundedAcceleration : currentAirborneAcceleration;

        Vector3 result = Vector3.MoveTowards(state.CurrentVelocity, targetVelocity, acceleration);
        result.y = state.CurrentVelocity.y;
        return result;
    }

    private static float CalculateAcceleration(float acceleration, ref MovementState state)
    {
        return 1 / acceleration * state.MaxSpeed * Time.deltaTime;
    }

    private static bool IsAccelerating(Vector3 targetVelocity)
    {
        return targetVelocity != Vector3.zero;
    }
}
