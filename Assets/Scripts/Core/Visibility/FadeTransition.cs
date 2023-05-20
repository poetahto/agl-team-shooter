using ElRaccoone.Tweens;
using UnityEngine;
using IEnumerator = System.Collections.IEnumerator;

namespace Core
{
    public class SlideUpTransition : VisibilityTransitionBase
    {
        private const float Offset = 50f;

        private readonly Transform _transform;
        private readonly float _duration;
        private readonly float _initialY;

        public SlideUpTransition(Transform transform, float duration = 0.1f, bool startVisible = false) : base(startVisible)
        {
            _transform = transform;
            _duration = duration;
            _initialY = transform.position.y;
        }

        protected override IEnumerator ShowCoroutine()
        {
            yield return _transform
                .TweenPositionY(_initialY, _duration)
                .SetFrom(_initialY - Offset)
                .SetEaseQuadOut()
                .Yield();
        }

        protected override IEnumerator HideCoroutine()
        {
            yield return _transform
                .TweenPositionY(_initialY + Offset, _duration)
                .SetEaseQuadIn()
                .Yield();
        }
    }

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
            yield return _canvasGroup.TweenCanvasGroupAlpha(1, _duration).Yield();
        }

        protected override IEnumerator HideCoroutine()
        {
            yield return _canvasGroup.TweenCanvasGroupAlpha(0, _duration).Yield();
        }
    }
}
