﻿using UnityEngine;

[CreateAssetMenu]
public class SourceMovementStrategy : MovementStrategy
{
    [Header("Settings")]
    [SerializeField] public float noFrictionJumpWindow = 0.1f;
    [SerializeField] public float friction = 5.5f;
    [SerializeField] public float airAcceleration = 40f;
    [SerializeField] public float groundAcceleration = 50f;
    [SerializeField] public float maxAirSpeed = 1f;

    private Vector3 _velocity;

    public override Vector3 CalculateVelocity(ref MovementState state)
    {
        _velocity = state.CurrentVelocity;

        return state.IsGrounded
            ? MoveGround(ref state)
            : MoveAir(ref state);
    }

    private Vector3 MoveGround(ref MovementState state)
    {
        float speed = state.CurrentSpeed;

        if (speed != 0 && state.TimeSpentOnGround > noFrictionJumpWindow)
        {
            float drop = speed * friction * Time.deltaTime;
            _velocity *= Mathf.Max(speed - drop, 0) / speed;
        }

        return Accelerate(groundAcceleration, state.MaxSpeed, ref state);
    }

    private Vector3 MoveAir(ref MovementState state)
    {
        return Accelerate(airAcceleration, maxAirSpeed, ref state);
    }

    private Vector3 Accelerate(float acceleration, float maxVelocity, ref MovementState state)
    {
        float projVel = Vector3.Dot(_velocity, state.InputDirection.normalized);
        float accelVel = acceleration * Time.deltaTime;

        if (projVel + accelVel > maxVelocity)
            accelVel = Mathf.Max(maxVelocity - projVel, 0);

        return _velocity + state.InputDirection * accelVel;
    }
}
