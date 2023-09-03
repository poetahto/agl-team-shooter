using UnityEngine;

namespace Core
{
    public static class PhysicsExtensions
    {
        public static bool TryGetComponentWithRigidbody<T>(this Collider collider, out T value)
        {
            bool hasRigidbody = collider.attachedRigidbody != null;

            if (hasRigidbody && collider.attachedRigidbody.TryGetComponent(out value))
                return true;

            else if (collider.TryGetComponent(out value))
                return true;

            return false;
        }

        public static GameObject GetRootGameObject(this Collider collider)
        {
            return collider.attachedRigidbody == null
                ? collider.gameObject
                : collider.attachedRigidbody.gameObject;
        }
    }
}
