using System;
using UnityEngine;
using UnityEngine.Assertions;

// - needs to provide more collision information for fall damage to be implemented
// - overall hard to understand, clean code

namespace poetools.Core
{
    public class GroundCheck : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The downwards direction used to check if we are grounded.")]
        private Vector3 gravityDirection = Vector3.down;

        [SerializeField]
        private Vector3 offset = Vector3.zero;

        [SerializeField]
        [Tooltip("How steep a slope we can climb without slipping.")]
        private float slopeLimitDegrees = 45f;

        [SerializeField]
        [Tooltip("Draws debug information to the screen.")]
        private bool showDebug;

        private void Update()
        {
            UpdateIsGrounded();
        }

        public event Action OnTouchGround, OnLeaveGround;

        private void UpdateIsGrounded()
        {
            WasGroundedLastFrame = IsGrounded;
            CheckIsGrounded();

            if (JustEntered)
            {
                OnTouchGround?.Invoke();
                TimeSpentFalling = 0;
            }

            if (JustExited)
            {
                OnLeaveGround?.Invoke();
                TimeSpentGrounded = 0;
            }

            if (IsGrounded)
                TimeSpentGrounded += Time.deltaTime;

            else TimeSpentFalling += Time.deltaTime;
        }

        #region Useful Data

        // todo: All of this stuff is useful: its just not required by the interface right now, maybe create a new one?

        private bool _isGrounded;
        public bool IsGrounded => _isGrounded;
        public float AirTime => TimeSpentFalling;
        public float GroundTime => TimeSpentGrounded;

        public bool WasGroundedLastFrame { get; set; }
        public float TimeSpentGrounded { get; set; }
        public float TimeSpentFalling { get; set; }

        public bool JustEntered => IsGrounded && !WasGroundedLastFrame;
        public bool JustExited => !IsGrounded && WasGroundedLastFrame;

        public Vector3 ContactNormal { get; set; }
        public Collider ConnectedCollider { get; set; }

        #endregion

        private Vector3 _previousPosition;
        private Vector3 _currentPosition;
        private Vector3 _velocity;

        public float groundDistance = 0.1f;

        private const int MaxHits = 10;
        private RaycastHit[] _hits = new RaycastHit[MaxHits];

        private void CheckIsGrounded()
        {
            int hits = Physics.BoxCastNonAlloc(transform.position + offset, new Vector3(0.25f, Mathf.Abs(groundDistance/2), 0.25f) * 0.99f, -transform.up, _hits, Quaternion.identity,groundDistance/2);

            Assert.IsTrue(hits <= MaxHits);

            int bestFit = -1;
            float closestDistance = float.PositiveInfinity;

            for (int i = 0; i < hits; i++)
            {
                var cur = _hits[i];

                // We cannot stand on triggers, so early out.
                if (cur.collider.isTrigger || cur.transform == transform)
                    continue;

                // We can only stand on slopes with the desired steepness
                Vector3 upDirection = -gravityDirection;
                Vector3 normalDirection = cur.normal;
                float slopeAngle = Vector3.Angle(upDirection, normalDirection);

                // We only want to check the nearest collider we hit.
                if (slopeAngle <= slopeLimitDegrees && cur.distance < closestDistance)
                    bestFit = i;
            }

            if (bestFit >= 0)
            {
                _isGrounded = true;
                ConnectedCollider = _hits[bestFit].collider;
                ContactNormal = _hits[bestFit].normal;
            }
            else
            {
                _isGrounded = false;
                ConnectedCollider = null;
                ContactNormal = Vector3.zero;
            }
        }

        #region Debug

        private void OnGUI()
        {
            if (showDebug)
            {
                string connectedCollider = ConnectedCollider ? ConnectedCollider.name : "None";

                GUILayout.Label($"IsGrounded: {IsGrounded}");
                GUILayout.Label($"Was Grounded Last Frame: {WasGroundedLastFrame}");
                GUILayout.Label($"Connected Collider: {connectedCollider}");
                GUILayout.Label($"Contact Normal: {ContactNormal}");
                GUILayout.Label($"Time spent grounded: {TimeSpentGrounded}");
                GUILayout.Label($"Time spent falling: {TimeSpentFalling}");
                GUILayout.Label($"Velocity: {_velocity}");
            }
        }

        #endregion
    }
}
