using System;
using UnityEngine;

namespace Core
{
    public class DisableTransition : IVisibilityTransition
    {
        private readonly GameObject _target;

        public event Action<bool> VisibilityChanged;

        public DisableTransition(GameObject target, bool startVisible = false)
        {
            _target = target;
            this.SetVisible(startVisible);
        }

        public bool IsVisible { get; private set; }

        public void Show()
        {
            _target.SetActive(true);
            IsVisible = true;
            VisibilityChanged?.Invoke(true);
        }

        public void Hide()
        {
            _target.SetActive(false);
            IsVisible = false;
            VisibilityChanged?.Invoke(false);
        }
    }
}