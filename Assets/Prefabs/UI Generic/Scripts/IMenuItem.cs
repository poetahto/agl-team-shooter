using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Poetools.UI
{
    /// <summary>
    /// Some item that can be seen and interacted with in a menu.
    /// </summary>
    public interface IMenuItem
    {
        /// <summary>
        /// Given a pre-existing object, hook into and apply
        /// the desired logic to it's components, if possible.
        /// </summary>
        /// <param name="gameObject"></param>
        /// <returns>A disposable that can be used to unbind this item.</returns>
        IDisposable BindTo(GameObject gameObject);

        /// <summary>
        /// Create a new object with the components needed for binding.
        /// </summary>
        /// <returns>The object that was created.</returns>
        GameObject Generate();
    }
}
