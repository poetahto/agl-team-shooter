using System.Collections;
using ElRaccoone.Tweens;
using UnityEngine;

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
            if (_transform != null)
                yield return _transform.TweenPositionY(_initialY, _duration)
                    .SetFrom(_initialY - Offset)
                    .SetEaseQuadOut()
                    .Yield();
        }

        protected override IEnumerator HideCoroutine()
        {
            if (_transform != null)
            {
                yield return _transform.TweenPositionY(_initialY + Offset, _duration)
                    .SetEaseQuadIn()
                    .Yield();

                _transform.position = new Vector3(_transform.position.x, _initialY, _transform.position.z);
            }
        }
    }
}
