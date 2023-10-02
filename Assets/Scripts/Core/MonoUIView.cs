using UnityEngine;

namespace Gameplay
{
    public abstract class MonoUIView<T> : MonoBehaviour where T : Object
    {
        [SerializeField]
        private bool clearOnAwake = true;

        [SerializeField]
        private T autoBind;

        private void Awake()
        {
            if (clearOnAwake)
                ClearBindings();
        }

        private void Start()
        {
            if (autoBind != null)
                BindTo(autoBind);
        }

        public abstract void BindTo(T instance);
        public abstract void ClearBindings();
    }
}
