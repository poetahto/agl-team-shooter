using System;
using System.Collections;
using UniRx;

namespace Core
{
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
}
