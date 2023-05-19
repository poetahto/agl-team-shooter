using System;
using UnityEngine;

namespace Poetools.UI
{
    /// <summary>
    /// A container that associates several <see cref="IMenuItem"/> with <see cref="GameObject"/>.
    /// </summary>
    public interface IMenuBuilder
    {
        /// <summary>
        /// Register a new item to be shown in this menu.
        /// </summary>
        /// <param name="id">A unique identifier for this item. Usage of this parameter varies by implementation.</param>
        /// <param name="item">The item that should be shown.</param>
        /// <returns>This menu builder.</returns>
        IMenuBuilder Register(string id, IMenuItem item);

        /// <summary>
        /// Traverses a root GameObject and applies registered settings to all objects it can find.
        /// </summary>
        /// <param name="root">The object whose children will be searched for valid objects.</param>
        /// <returns>A disposable that can be disposed to unregister all menu items.</returns>
        IDisposable Build(GameObject root);
    }
}
