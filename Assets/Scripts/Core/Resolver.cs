using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core
{
    public class Resolver<T> where T : Component
    {
        private readonly Stack<Func<bool>> _conditionals = new Stack<Func<bool>>();
        private readonly GameObject _root;
        private T _value;

        public Resolver(GameObject root, T value = null)
        {
            _root = root;
            _value = value;
        }

        public bool ShouldResolve => (!_conditionals.TryPop(out Func<bool> condition) || condition.Invoke()) && _value == null;

        public Resolver<T> FromSelf()
        {
            if (ShouldResolve)
                _value = _root.GetComponent<T>();

            return this;
        }

        public Resolver<T> FromChildren(bool includeInactive = true, string name = null)
        {
            if (ShouldResolve)
            {
                if (name != null)
                {
                    _value = _root.GetComponentsInChildren<T>(includeInactive)
                        .FirstOrDefault(target => target.name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
                }
                else _value = _root.GetComponentInChildren<T>(includeInactive);
            }

            return this;
        }

        public Resolver<T> FromComponent<TComponent>(Func<TComponent, T> selector) where TComponent : Component
        {
            if (ShouldResolve)
            {
                TComponent component = _root.GetComponentInChildren<TComponent>();

                if (component != null)
                    _value = selector.Invoke(component);
            }

            return this;
        }

        public Resolver<T> FromNewComponent(Action<T> setup = null, GameObject target = null)
        {
            if (target == null)
                target = _root;

            if (ShouldResolve)
            {
                _value = target.AddComponent<T>();
                setup?.Invoke(_value);
            }

            return this;
        }

        public Resolver<T> FromCustom(Func<T> resolver)
        {
            if (ShouldResolve)
                _value = resolver.Invoke();

            return this;
        }

        public Resolver<T> If(Func<bool> condition)
        {
            _conditionals.Push(condition);
            return this;
        }

        public T Complete()
        {
            return _value;
        }

        public static T SimpleResolve(GameObject root, T component = null)
        {
            return new Resolver<T>(root, component).FromSelf().FromChildren().Complete();
        }
    }

    public static class GameObjectExtensions
    {
        public static Resolver<T> Resolve<T>(this GameObject root, T component = null) where T : Component
        {
            return new Resolver<T>(root, component);
        }

        public static T SimpleResolve<T>(this GameObject root, T component = null) where T : Component
        {
            return Resolver<T>.SimpleResolve(root, component);
        }
    }
}
