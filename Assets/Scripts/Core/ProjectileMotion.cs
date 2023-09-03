namespace Application.Gameplay.Combat
{
    using UnityEngine;

    /// <summary>
    /// Provides helper utilities for computing projectile motion data.
    /// </summary>
    public static class ProjectileMotion
    {
        /// <summary>
        /// Calculates the launch velocity of an explosion.
        /// </summary>
        /// <param name="source">The origin of the explosion.</param>
        /// <param name="target">The position of the object being affected by the explosion.</param>
        /// <param name="strength">The knockback intensity of the explosion.</param>
        /// <param name="upwardBoost">The extra upward force the explosion imparts.</param>
        /// <returns>The velocity that should be applied by this explosion.</returns>
        public static Vector3 GetExplosionVelocity(Vector3 source, Vector3 target, float strength, float upwardBoost = 0)
        {
            var sourceToTarget = new Vector3(target.x, 0, target.z) - new Vector3(source.x, 0, source.z);
            return sourceToTarget.normalized * strength + Vector3.up * upwardBoost;
        }

        /// <summary>
        /// Calculates the velocity needed to launch a projectile between two points, over a set time.
        /// </summary>
        /// <param name="from">The starting point.</param>
        /// <param name="to">The ending point.</param>
        /// <param name="duration">How long the projectile should take in-flight.</param>
        /// <returns>The velocity to assign the projectile.</returns>
        public static Vector3 GetLaunchVelocity(Vector3 from, Vector3 to, float duration = 1)
        {
            float distance = GetFlatDistance(from, to);

            // Local-space launch velocity, derived from projectile motion equations
            float initialY = (to.y - from.y - 0.5f * Physics.gravity.y * duration * duration) / duration;
            float initialZ = distance / duration;
            Vector3 localVelocity = new Vector3(0, initialY, initialZ);

            // Rotate the local-space velocity so its aimed the correct direction.
            Vector3 direction = (new Vector3(to.x, 0, to.z) - new Vector3(from.x, 0, from.z)).normalized;
            Quaternion rotation = Quaternion.LookRotation(direction);
            return rotation * localVelocity;
        }

        private static float GetFlatDistance(Vector3 from, Vector3 to)
        {
            return (new Vector2(to.x, to.z) - new Vector2(from.x, from.z)).magnitude;
        }
    }
}
