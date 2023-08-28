using UnityEngine;

namespace Gameplay
{
    public abstract class MonoUIView<T> : MonoBehaviour
    {
        public abstract void BindTo(T instance);
        public abstract void ClearBindings();
    }
}