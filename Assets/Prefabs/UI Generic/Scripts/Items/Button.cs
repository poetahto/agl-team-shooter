using System;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Poetools.UI.Items
{
    /// <summary>
    /// Logic for a button menu item.
    /// </summary>
    public class Button : IMenuItem
    {
        private readonly Action _pressed;
        private readonly string _title;

        /// <summary>
        /// Creates a new instance of <see cref="Button"/>
        /// </summary>
        /// <param name="pressed">The callback to invoke when the button is pressed.</param>
        /// <param name="title">The text that should be displayed within the button.</param>
        public Button(Action pressed, string title = null)
        {
            _pressed = pressed;
            _title = title;
        }

        /// <inheritdoc />
        public GameObject Generate()
        {
            return Object.Instantiate(UIPrefabs.Instance.button);
        }

        /// <inheritdoc />
        public IDisposable BindTo(GameObject gameObject)
        {
            if (!gameObject.TryGetComponent(out UnityEngine.UI.Button button))
                throw new Exception("Button prefab did not have a UnityEngine.UI.Button!");

            { // Set the title text of the button, if it exists.
                var titleText = gameObject.GetComponentInChildren<TMP_Text>();

                if (titleText != null && _title != null)
                    titleText.text = _title;
            }

            button.onClick.AddListener(_pressed.Invoke);
            return new DisposableAction(() => button.onClick.RemoveListener(_pressed.Invoke));
        }
    }
}
