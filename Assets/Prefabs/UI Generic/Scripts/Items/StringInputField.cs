using System;
using Poetools.UI.Serialization;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Poetools.UI.Items
{
    /// <summary>
    /// Logic for a string-based input field.
    /// </summary>
    public class StringInputField : IMenuItem
    {
        private readonly Action<string> _changed;
        private readonly ISerializer _serializer;

        /// <summary>
        /// Creates a new instance of <see cref="StringInputField"/>
        /// </summary>
        /// <param name="changed">The callback to invoke when the text input changes.</param>
        /// <param name="serializer">The serializer to use for saving values. Defaults to PlayerPrefs.</param>
        public StringInputField(Action<string> changed, ISerializer serializer = null)
        {
            _changed = changed;
            _serializer = serializer ?? PlayerPrefsSerializer.Instance;
        }

        /// <inheritdoc />
        public GameObject Generate()
        {
            return Object.Instantiate(UIPrefabs.Instance.stringInput);
        }

        /// <inheritdoc />
        public IDisposable BindTo(GameObject gameObject)
        {
            if (!gameObject.TryGetComponent(out TMP_InputField inputField))
                throw new Exception("String Input Field prefab did not have a TMP_InputField!");

            string key = $"string_input_field_{gameObject.name}";

            // Update to use initial serialized value.
            string initialValue = _serializer.Read<string>(key);
            inputField.text = initialValue;
            SetValue(key, initialValue);

            // Use local function so we can easily reference the key, and easily unregister.
            void HandleValueChanged(string value) => SetValue(key, value);
            inputField.onValueChanged.AddListener(HandleValueChanged);
            return new DisposableAction(() => inputField.onValueChanged.RemoveListener(HandleValueChanged));
        }

        private void SetValue(string key, string value)
        {
            _changed.Invoke(value);
            _serializer.Write(key, value);
        }
    }
}
