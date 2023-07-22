using System;
using System.Collections.Generic;
using Poetools.UI.Serialization;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Poetools.UI.Items
{
    /// <summary>
    /// Logic for a dropdown menu item.
    /// </summary>
    public class Dropdown : IMenuItem
    {
        private readonly List<Action> _actions = new List<Action>();
        private readonly List<TMP_Dropdown.OptionData> _data = new List<TMP_Dropdown.OptionData>();
        private readonly ISerializer _serializer;

        /// <summary>
        /// Creates a new instance of <see cref="Dropdown"/>
        /// </summary>
        /// <param name="serializer">The serializer to use for saving values. Defaults to PlayerPrefs.</param>
        public Dropdown(ISerializer serializer = null)
        {
            _serializer = serializer ?? PlayerPrefsSerializer.Instance;
        }

        /// <inheritdoc />
        public GameObject Generate()
        {
            return Object.Instantiate(UIPrefabs.Instance.dropdown);
        }

        /// <inheritdoc />
        public IDisposable BindTo(GameObject gameObject)
        {
            if (!gameObject.TryGetComponent(out TMP_Dropdown dropdown))
                throw new Exception("Dropdown prefab did not have a TMP_Dropdown!");

            dropdown.ClearOptions();
            dropdown.AddOptions(_data);
            string key = $"dropdown_{gameObject.name}";

            // Update to use initial serialized value.
            int initialValue = _serializer.Read<int>(key);
            dropdown.value = initialValue;
            SetValue(key, initialValue);

            // Use local function so we can easily reference the key, and easily unregister.
            void HandleValueChanged(int value) => SetValue(key, value);
            dropdown.onValueChanged.AddListener(HandleValueChanged);
            return new DisposableAction(() => dropdown.onValueChanged.RemoveListener(HandleValueChanged));
        }

        private void SetValue(string key, int value)
        {
            _actions[value].Invoke();
            _serializer.Write(key, value);
        }

        // Option Management
        // - A bunch of utility methods for easily adding options to the dropdown.

        /// <summary>
        /// Adds a new option to this dropdown.
        /// </summary>
        /// <param name="label">The text to be displayed.</param>
        /// <param name="action">The action to be executed when this option is selected.</param>
        public void AddOption(string label, Action action)
        {
            _data.Add(new TMP_Dropdown.OptionData(label));
            _actions.Add(action);
        }

        /// <summary>
        /// Adds a set of options to this dropdown.
        /// </summary>
        /// <param name="collection">The collection of data to draw from.</param>
        /// <param name="labelSelector">A function that extracts a label from the data.</param>
        /// <param name="actionSelector">A function that extracts an action from the data.</param>
        /// <typeparam name="T">The type of data to iterate over.</typeparam>
        public void AddOptions<T>(IEnumerable<T> collection,
            Func<T, string> labelSelector, Func<T, Action> actionSelector)
        {
            foreach (var item in collection)
            {
                string label = labelSelector.Invoke(item);
                Action action = actionSelector.Invoke(item);
                AddOption(label, action);
            }
        }

        /// <summary>
        /// Adds a new option to this dropdown.
        /// </summary>
        /// <param name="label">The text to be displayed.</param>
        /// <param name="action">The action to be executed when this option is selected.</param>
        /// <returns>The modified dropdown.</returns>
        public Dropdown WithOption(string label, Action action)
        {
            AddOption(label, action);
            return this;
        }

        /// <summary>
        /// Adds a set of options to this dropdown.
        /// </summary>
        /// <param name="collection">The collection of data to draw from.</param>
        /// <param name="labelSelector">A function that extracts a label from the data.</param>
        /// <param name="actionSelector">A function that extracts an action from the data.</param>
        /// <typeparam name="T">The type of data to iterate over.</typeparam>
        /// <returns>The modified dropdown.</returns>
        public Dropdown WithOptions<T>(IEnumerable<T> collection,
            Func<T, string> labelSelector, Func<T, Action> actionSelector)
        {
            AddOptions(collection, labelSelector, actionSelector);
            return this;
        }

        /// <summary>
        /// Creates a new dropdown from the provided options.
        /// </summary>
        /// <param name="collection">The collection of data to draw from.</param>
        /// <param name="labelSelector">A function that extracts a label from the data.</param>
        /// <param name="actionSelector">A function that extracts an action from the data.</param>
        /// <param name="serializer">The serializer to use for this dropdown.</param>
        /// <typeparam name="T">The type of data to iterate over.</typeparam>
        /// <returns>The newly created dropdown.</returns>
        public static Dropdown FromOptions<T>(IEnumerable<T> collection,
            Func<T, string> labelSelector, Func<T, Action> actionSelector,
            ISerializer serializer = null)
        {
            return new Dropdown(serializer).WithOptions(collection, labelSelector, actionSelector);
        }

        /// <summary>
        /// Creates a new dropdown from the provided options.
        /// </summary>
        /// <param name="serializer">The serializer to use for this dropdown.</param>
        /// <param name="options">The options used for creating this dropdown.</param>
        /// <returns>The newly created dropdown.</returns>
        public static Dropdown FromOptions(ISerializer serializer = null, params (string label, Action action)[] options)
        {
            return new Dropdown(serializer).WithOptions(options,
                option => option.label,
                option => option.action
            );
        }

        /// <summary>
        /// Creates a new dropdown from the provided options.
        /// </summary>
        /// <param name="collection">A list of strings that will be the dropdown labels.</param>
        /// <param name="action">The action that should run, depending on which index of the collection was selected.</param>
        /// <param name="serializer">The serializer to use for this dropdown.</param>
        /// <returns></returns>
        public static Dropdown FromOptions(IEnumerable<string> collection, Action<int> action, ISerializer serializer = null)
        {
            var result = new Dropdown(serializer);
            int index = 0;

            foreach (var item in collection)
            {
                int cur = index;
                result.AddOption(item, () => action.Invoke(cur));
                index++;
            }

            return result;
        }
    }
}
