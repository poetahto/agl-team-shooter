using System;
using System.Collections;
using UniRx;
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

    public abstract class VisibilityTransitionBase : IVisibilityTransition
    {
        private IDisposable _animation;

        public event Action<bool> VisibilityChanged;

        protected VisibilityTransitionBase(bool startVisible)
        {
            SetVisible(startVisible);
        }

        public bool IsVisible { get; private set; }

        public void Show()
        {
            IEnumerator Coroutine()
            {
                SetVisible(true);
                yield return Observable.FromCoroutine(ShowCoroutine).ToYieldInstruction();
            }

            _animation?.Dispose();
            _animation = Observable.FromCoroutine(Coroutine).Subscribe();
        }

        public void Hide()
        {
            IEnumerator Coroutine()
            {
                yield return Observable.FromCoroutine(HideCoroutine).ToYieldInstruction();
                SetVisible(false);
            }

            _animation?.Dispose();
            _animation = Observable.FromCoroutine(Coroutine).Subscribe();
        }

        private void SetVisible(bool visible)
        {
            IsVisible = visible;
            VisibilityChanged?.Invoke(visible);
        }

        protected abstract IEnumerator ShowCoroutine();
        protected abstract IEnumerator HideCoroutine();
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
