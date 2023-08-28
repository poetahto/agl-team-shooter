using UnityEngine;

namespace Gameplay
{
    public abstract class MonoUIView<T> : MonoBehaviour
    {
        [SerializeField]
        private bool clearOnAwake = true;

        private void Awake()
        {
            if (clearOnAwake)
                ClearBindings();
        }

        public abstract void BindTo(T instance);
        public abstract void ClearBindings();
    }
}
