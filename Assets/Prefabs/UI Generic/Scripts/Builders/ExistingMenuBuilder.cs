using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Poetools.UI.Items;
using UnityEngine;

namespace Poetools.UI.Builders
{
    /// <summary>
    /// A menu builder that searches it's children for targets to apply logic on.
    /// Will not automatically instantiate items, they must already exist (and have a unique name).
    /// </summary>
    public class ExistingMenuBuilder : IMenuBuilder
    {
        private readonly List<ItemData> _data = new List<ItemData>();

        /// <inheritdoc />
        public IMenuBuilder Register(string id, IMenuItem item)
        {
            _data.Add(new ItemData{Id = id, Item = item});
            return this;
        }

        /// <inheritdoc />
        public IDisposable Build(GameObject root)
        {
            var disposables = new List<IDisposable>();

            foreach (var data in _data)
            {
                GameObject target = FindRecursive(root, data.Id);

                if (target != null)
                    disposables.Add(data.Item.BindTo(target));
            }

            return new DisposableAction(() => disposables.ForEach(disposable => disposable.Dispose()));
        }

        // A recursive, deep searching version of Find().
        [CanBeNull]
        private static GameObject FindRecursive(GameObject obj, string id)
        {
            // Base case: the name matches.
            if (obj.name == id)
                return obj;

            for (int i = 0; i < obj.transform.childCount; i++)
            {
                var child = obj.transform.GetChild(i);
                var result = FindRecursive(child.gameObject, id);

                if (result != null)
                    return result;
            }

            return null;
        }

        // Represents a registered item.
        private class ItemData
        {
            public string Id;
            public IMenuItem Item;
        }
    }
}
