using System;
using Poetools.UI.Serialization;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Poetools.UI.Items
{
    /// <summary>
    /// Logic for a slider menu item, using floating point values.
    /// </summary>
    public class Slider : IMenuItem
    {
        private readonly Action<float> _changed;
        private readonly ISerializer _serializer;

        /// <summary>
        /// Creates a new instance of <see cref="Slider"/>
        /// </summary>
        /// <param name="changed">The callback to invoke when the slider changes values.</param>
        /// <param name="serializer">The serializer to use for saving values. Defaults to PlayerPrefs.</param>
        public Slider(Action<float> changed, ISerializer serializer = null)
        {
            _changed = changed;
            _serializer = serializer ?? PlayerPrefsSerializer.Instance;
        }

        /// <inheritdoc />
        public GameObject Generate()
        {
            return Object.Instantiate(UIPrefabs.Instance.slider);
        }

        /// <inheritdoc />
        public IDisposable BindTo(GameObject gameObject)
        {
            if (!gameObject.TryGetComponent(out UnityEngine.UI.Slider slider))
                throw new Exception("Slider prefab did not have a UnityEngine.UI.Slider!");

            string key = $"slider_{gameObject.name}";

            // Update to use initial serialized value.
            float initialValue = _serializer.Read<float>(key);
            slider.value = initialValue;
            SetValue(key, initialValue);

            // Use local function so we can easily reference the key, and easily unregister.
            void HandleValueChanged(float value) => SetValue(key, value);
            slider.onValueChanged.AddListener(HandleValueChanged);
            return new DisposableAction(() => slider.onValueChanged.RemoveListener(HandleValueChanged));

        }

        private void SetValue(string key, float value)
        {
            _changed.Invoke(value);
            _serializer.Write(key, value);
        }
    }
}
