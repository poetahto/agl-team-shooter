using System;
using System.Collections.Generic;
using Poetools.UI.Items;
using UnityEngine;

namespace Poetools.UI.Builders
{
    /// <summary>
    /// A menu builder that will automatically instantiate the registered items.
    /// This builder is best used in combination with one of the built-in LayoutGroup components.
    /// </summary>
    public class AutoMenuBuilder : IMenuBuilder
    {
        private readonly List<ItemData> _data = new List<ItemData>();
        private readonly string _uniqueName;

        /// <summary>
        /// Creates a new instance of <see cref="AutoMenuBuilder"/>
        /// </summary>
        /// <param name="uniqueName">A unique name that is prepended to each registered id.</param>
        public AutoMenuBuilder(string uniqueName = "")
        {
            _uniqueName = uniqueName;
        }

        /// <inheritdoc />
        public IMenuBuilder Register(string id, IMenuItem item)
        {
            string n = _uniqueName == string.Empty ? _uniqueName : $"{_uniqueName}_";
            // Registering doesn't actually create the item yet: we just queue it up for later.
            _data.Add(new ItemData{Id = $"{n}{id}", Item = item});
            return this;
        }

        /// <inheritdoc />
        public IDisposable Build(GameObject root)
        {
            var disposables = new List<IDisposable>();

            // Traverse all queued items and actually instantiate + bind them.
            foreach (var data in _data)
            {
                GameObject itemInstance = data.Item.Generate();
                itemInstance.transform.SetParent(root.transform);
                itemInstance.name = data.Id;

                disposables.Add(data.Item.BindTo(itemInstance));
            }

            return new DisposableAction(() => disposables.ForEach(disposable => disposable.Dispose()));
        }

        // Represents a registered item.
        private class ItemData
        {
            public string Id;
            public IMenuItem Item;
        }
    }
}
