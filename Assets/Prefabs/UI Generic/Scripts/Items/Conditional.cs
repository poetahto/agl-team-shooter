using System;
using UnityEngine;

namespace Poetools.UI.Items
{
    public class Conditional : IMenuItem
    {
        private readonly IMenuItem _item;
        private readonly bool _shouldShow;

        public Conditional(IMenuItem item, bool shouldShow)
        {
            _item = item;
            _shouldShow = shouldShow;
        }

        public IDisposable BindTo(GameObject gameObject)
        {
            var result = _item.BindTo(gameObject);
            gameObject.SetActive(_shouldShow);
            return result;
        }

        public GameObject Generate()
        {
            return _item.Generate();
        }
    }
}
