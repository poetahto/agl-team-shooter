using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core
{
    public interface IVisibilityTransition
    {
        void Show();
        void Hide();
        bool IsVisible { get; }
        event Action<bool> VisibilityChanged;
    }

    public static class VisibilityTransitionExtensions
    {
        public static void SetVisible(this IVisibilityTransition transition, bool isVisible)
        {
            if (isVisible)
                transition.Show();

            else transition.Hide();
        }

        public static void Toggle(this IVisibilityTransition transition)
        {
            transition.SetVisible(!transition.IsVisible);
        }

        public static void HideThenDestroy(this IVisibilityTransition transition, params GameObject[] objects)
        {
            void HandleVisibilityChange(bool isVisible)
            {
                if (!isVisible)
                {
                    transition.VisibilityChanged -= HandleVisibilityChange;

                    foreach (GameObject obj in objects)
                        Object.Destroy(obj);
                }
            }

            transition.Hide();
            transition.VisibilityChanged += HandleVisibilityChange;
        }
    }
}
