using System;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Poetools.UI.Items
{
    /// <summary>
    /// A wrapper for other menu items that displays some text alongside the original content.
    /// </summary>
    public class Label : IMenuItem
    {
        private readonly string _text;
        private readonly IMenuItem _target;

        /// <summary>
        /// Creates a new instance of <see cref="Label"/>.
        /// </summary>
        /// <param name="text">The text that should be displayed alongside the target.</param>
        /// <param name="target">A menu item that should be annotated with text.</param>
        public Label(string text, IMenuItem target)
        {
            _text = text;
            _target = target;
        }

        /// <inheritdoc />
        public GameObject Generate()
        {
            // This generates the parent of the label: the actual content and text will be children.
            return new GameObject(string.Empty, typeof(RectTransform));
        }

        /// <inheritdoc />
        public IDisposable BindTo(GameObject gameObject)
        {
            GameObject itemInstance = _target.Generate();
            GameObject labelInstance = Object.Instantiate(UIPrefabs.Instance.text);
            Transform parentTransform = gameObject.transform;
            parentTransform.localScale = Vector3.one;

            { // Create and set up the text label object.
                var t = labelInstance.GetComponent<RectTransform>();
                t.SetParent(parentTransform);
                t.localScale = Vector3.one;
                t.name = "label";
                t.GetComponentInChildren<TMP_Text>().text = _text;
                t.anchorMin = new Vector2(0, 0);
                t.anchorMax = new Vector2(0.5f, 1);
                t.anchoredPosition = Vector2.zero;
                t.sizeDelta = Vector2.zero;
            }

            { // Create and set up the menu item object.
                var t = itemInstance.GetComponent<RectTransform>();
                t.SetParent(parentTransform);
                t.localScale = Vector3.one;
                t.name = gameObject.name;
                t.anchorMin = new Vector2(0.5f, 0.5f);
                t.anchorMax = new Vector2(1, 0.5f);

                // We only want to expand and fill horizontally, not vertically.
                // This lets stuff like sliders still look good.
                t.anchoredPosition = new Vector2(0, t.anchoredPosition.y);
                t.sizeDelta = new Vector2(0, t.sizeDelta.y);
            }

            gameObject.name = $"PARENT: {gameObject.name}";
            return _target.BindTo(itemInstance);
        }
    }

    /// <summary>
    /// Helper extensions for making label application easier and more readable.
    /// </summary>
    public static class LabelExtensions
    {
        /// <summary>
        /// Display some text alongside this menu item.
        /// </summary>
        /// <param name="item">The original menu item content.</param>
        /// <param name="text">The text that should be displayed alongside the content.</param>
        /// <returns>A new label menu item.</returns>
        public static Label WithLabel(this IMenuItem item, string text) => new Label(text, item);
    }
}
