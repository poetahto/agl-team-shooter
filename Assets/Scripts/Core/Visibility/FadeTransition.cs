using System;
using System.Collections;
using System.Threading;
using UniRx;
using UnityEngine;

namespace Core
{
    public class FadeTransition : IVisibilityTransition
    {
        private readonly CanvasGroup _canvasGroup;
        private readonly float _duration;
        private IDisposable _animation;

        public event Action<bool> VisibilityChanged;

        public FadeTransition(CanvasGroup canvasGroup, float duration = 0.1f, bool startVisible = false)
        {
            _canvasGroup = canvasGroup;
            _duration = duration;

            // Applying default values to the canvas group.
            _canvasGroup.alpha = startVisible ? 1 : 0;
            _canvasGroup.interactable = startVisible;
            _canvasGroup.blocksRaycasts = startVisible;
        }

        public bool IsVisible { get; private set; }

        public void Show()
        {
            IEnumerator Coroutine(CancellationToken ct)
            {
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
                yield return FadeTo(1, ct);
                IsVisible = true;
                VisibilityChanged?.Invoke(true);
            }

            _animation?.Dispose();
            _animation = Observable.FromMicroCoroutine(Coroutine).Subscribe();
        }

        public void Hide()
        {
            IEnumerator Coroutine(CancellationToken ct)
            {
                yield return FadeTo(0, ct);
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
                IsVisible = false;
                VisibilityChanged?.Invoke(false);
            }

            _animation?.Dispose();
            _animation = Observable.FromMicroCoroutine(Coroutine).Subscribe();
        }

        private IEnumerator FadeTo(float value, CancellationToken ct)
        {
            while (Math.Abs(_canvasGroup.alpha - value) > 0.01f && !ct.IsCancellationRequested)
            {
                float t = 1 / _duration * Time.deltaTime;
                _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, value, t);
                yield return null;
            }

            if (!ct.IsCancellationRequested)
                _canvasGroup.alpha = value;
        }
    }
}
