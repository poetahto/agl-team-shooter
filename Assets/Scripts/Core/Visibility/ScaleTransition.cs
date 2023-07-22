using System.Collections;
using Cysharp.Threading.Tasks;
using ElRaccoone.Tweens;
using UnityEngine;

namespace Core
{
    public class ScaleTransition : VisibilityTransitionBase
    {
        private readonly Transform _transform;
        private readonly float _duration;
        private readonly Vector3 _initialScale;

        public ScaleTransition(Transform transform, float duration = 0.1f, bool startVisible = false) : base(
            startVisible)
        {
            _transform = transform;
            _duration = duration;
            _initialScale = transform.localScale;
            transform.localScale = startVisible ? _initialScale : Vector3.zero;
        }

        protected override IEnumerator ShowCoroutine()
        {
            if (_transform != null)
                _transform.TweenLocalScale(_initialScale, _duration).SetEaseQuadOut();

            yield return new WaitForSeconds(_duration);
        }

        protected override IEnumerator HideCoroutine()
        {
            if (_transform != null)
                _transform.TweenLocalScale(Vector3.zero, _duration).SetEaseQuadIn();

            yield return new WaitForSeconds(_duration);
        }
    }
}
