using Cysharp.Threading.Tasks;
using ElRaccoone.Tweens;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

namespace Core
{
    public class FadeTransition : VisibilityTransitionBase
    {
        private readonly CanvasGroup _canvasGroup;
        private readonly float _duration;

        public FadeTransition(CanvasGroup canvasGroup, float duration = 0.1f, bool startVisible = false) : base(startVisible)
        {
            _canvasGroup = canvasGroup;
            _duration = duration;
        }

        protected override IEnumerator ShowCoroutine()
        {
            if (_canvasGroup != null)
                yield return _canvasGroup.TweenCanvasGroupAlpha(1, _duration).Yield();
        }

        protected override IEnumerator HideCoroutine()
        {
            if (_canvasGroup != null)
                yield return _canvasGroup.TweenCanvasGroupAlpha(0, _duration).Yield();
        }
    }
}
