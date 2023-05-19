using UnityEngine;

namespace Core
{
    public interface IVisibilityTransition
    {
        public void Show();
        public void Hide();

        public BoolReactiveProperty IsVisible { get; }
    }

    public static class VisibilityTransitionExtensions
    {
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

        public DefaultVisibilityTransition(CanvasGroup target)
        {
            _target = target;
        }

        public BoolReactiveProperty IsVisible { get; } = new BoolReactiveProperty();

        public void Show()
        {
            _target.alpha = 1;
            _target.interactable = true;
            _target.blocksRaycasts = true;
            IsVisible.Value = true;
        }

        public void Hide()
        {
            _target.alpha = 0;
            _target.interactable = false;
            _target.blocksRaycasts = false;
            IsVisible.Value = false;
        }
    }
}
