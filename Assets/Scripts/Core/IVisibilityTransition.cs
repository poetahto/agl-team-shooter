using UnityEngine;

namespace Core
{
    public interface IVisibilityTransition
    {
        public BoolReactiveProperty IsVisible { get; }
    }

    public static class VisibilityTransitionExtensions
    {
        public static void SetVisible(this IVisibilityTransition transition, bool isVisible) => transition.IsVisible.Value = isVisible;
        public static void Hide(this IVisibilityTransition transition) => transition.SetVisible(false);
        public static void Show(this IVisibilityTransition transition) => transition.SetVisible(true);
        public static void Toggle(this IVisibilityTransition transition) => transition.SetVisible(!transition.IsVisible);

        public static void HideThenDestroy(this IVisibilityTransition transition, params GameObject[] objects)
        {
            void HandleVisibilityChange(bool isVisible)
            {
                if (!isVisible)
                {
                    transition.IsVisible.changed.RemoveListener(HandleVisibilityChange);

                    foreach (GameObject obj in objects)
                        Object.Destroy(obj);
                }
            }

            transition.Hide();
            transition.IsVisible.changed.AddListener(HandleVisibilityChange);
        }
    }

    public class DefaultVisibilityTransition : IVisibilityTransition
    {
        private readonly CanvasGroup _target;

        public DefaultVisibilityTransition(CanvasGroup target, bool startVisible = false)
        {
            _target = target;
            IsVisible.Value = startVisible;
            IsVisible.changed.AddListener(HandleVisibleChanged);
            HandleVisibleChanged(startVisible);
        }

        private void HandleVisibleChanged(bool isVisible)
        {
            _target.alpha = isVisible ? 1 : 0;
            _target.interactable = isVisible;
            _target.blocksRaycasts = isVisible;
        }

        public BoolReactiveProperty IsVisible { get; } = new BoolReactiveProperty();
    }
}
