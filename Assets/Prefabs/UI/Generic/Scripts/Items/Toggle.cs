using System;
using Poetools.UI.Serialization;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Poetools.UI.Items
{
    /// <summary>
    /// Logic for a toggle menu item.
    /// </summary>
    public class Toggle : IMenuItem
    {
        private readonly Action<bool> _changed;
        private readonly ISerializer _serializer;

        /// <summary>
        /// Creates a new instance of <see cref="Toggle"/>
        /// </summary>
        /// <param name="changed">The callback to invoke when the toggle changes value.</param>
        /// <param name="serializer">The serializer to use when saving values. Defaults to PlayerPrefs.</param>
        public Toggle(Action<bool> changed, ISerializer serializer = null)
        {
            _changed = changed;
            _serializer = serializer ?? PlayerPrefsSerializer.Instance;
        }

        /// <inheritdoc />
        public GameObject Generate()
        {
            return Object.Instantiate(UIPrefabs.Instance.toggle);
        }

        /// <inheritdoc />
        public IDisposable BindTo(GameObject gameObject)
        {
            if (!gameObject.TryGetComponent(out UnityEngine.UI.Toggle toggle))
                throw new Exception("Toggle prefab did not have a UnityEngine.UI.Toggle!");

            string key = $"toggle_{gameObject.name}";

            // Update to use initial serialized value.
            bool initialValue = _serializer.Read<bool>(key);
            toggle.isOn = initialValue;
            SetValue(key, initialValue);

            // Use local function so we can easily reference the key, and easily unregister.
            void HandleValueChanged(bool value) => SetValue(key, value);
            toggle.onValueChanged.AddListener(HandleValueChanged);
            return new DisposableAction(() => toggle.onValueChanged.RemoveListener(HandleValueChanged));
        }

        private void SetValue(string key, bool value)
        {
            _changed.Invoke(value);
            _serializer.Write(key, value);
        }
    }
}
