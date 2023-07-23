using UnityEngine;

[CreateAssetMenu]
public class FreeMovementStrategy : MovementStrategy
{
    [Header("Settings")]
    [SerializeField] public float groundedAcceleration = 0.125f;
    [SerializeField] public float groundedDeceleration = 0.125f;
    [SerializeField] public float airborneAcceleration = 0.3f;
    [SerializeField] public float airborneDeceleration = 0.3f;

    private bool IsAccelerating => _targetVelocity != Vector3.zero;

    private Vector3 _targetVelocity;

    public override Vector3 CalculateVelocity(ref MovementState state)
    {
        _targetVelocity = state.InputDirection * state.MaxSpeed;

        ClampVelocity(ref state);

        float currentGroundedAcceleration = IsAccelerating
            ? CalculateAcceleration(groundedAcceleration, ref state)
            : CalculateAcceleration(groundedDeceleration, ref state);

        float currentAirborneAcceleration = IsAccelerating
            ? CalculateAcceleration(airborneAcceleration, ref state)
            : CalculateAcceleration(airborneDeceleration, ref state);

        float acceleration = state.IsGrounded ? currentGroundedAcceleration : currentAirborneAcceleration;

        return Vector3.MoveTowards(state.CurrentVelocity, _targetVelocity, acceleration);
    }

    private void ClampVelocity(ref MovementState state)
    {
        if (_targetVelocity.x > 0)
            PositiveClampVelocity(ref state);

        if (_targetVelocity.z < 0)
            NegativeClampVelocity(ref state);
    }

    private void PositiveClampVelocity(ref MovementState state)
    {
        if (_targetVelocity.x >= state.MaxSpeed)
            _targetVelocity.x = state.CurrentVelocity.x;

        if (_targetVelocity.z >= state.MaxSpeed)
            _targetVelocity.z = state.CurrentVelocity.z;
    }

    private void NegativeClampVelocity(ref MovementState state)
    {
        if (_targetVelocity.x <= -state.MaxSpeed)
            _targetVelocity.x = state.CurrentVelocity.x;

        if (_targetVelocity.z <= -state.MaxSpeed)
            _targetVelocity.z = state.CurrentVelocity.z;
    }

    private float CalculateAcceleration(float acceleration, ref MovementState state)
    {
        return 1 / acceleration * state.MaxSpeed * Time.deltaTime;
    }
}
