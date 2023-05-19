using System;
using UnityEngine.Events;

namespace Core
{
    [Serializable]
    public class ReactiveProperty<T>
    {
        public UnityEvent<T> changed = new UnityEvent<T>();

        private T _value;

        public ReactiveProperty(T initialValue = default)
        {
            _value = initialValue;
        }

        public T Value
        {
            get => _value;
            set
            {
                if (!_value.Equals(value))
                {
                    _value = value;
                    changed.Invoke(value);
                }
            }
        }

        public static implicit operator T(ReactiveProperty<T> property) => property.Value;
    }

    public class BoolReactiveProperty : ReactiveProperty<bool> {}
}
