using System.Collections;
using ElRaccoone.Tweens;
using UnityEngine;

namespace Core
{
    public class SlideUpTransition : VisibilityTransitionBase
    {
        private readonly Transform _transform;
        private readonly float _duration;
        private readonly float _initialY;
        private readonly float _offset;

        public SlideUpTransition(Transform transform, float duration = 0.1f, float offset = 50f, bool startVisible = false) : base(startVisible)
        {
            _transform = transform;
            _duration = duration;
            _initialY = transform.position.y;
            _offset = offset;
        }

        protected override IEnumerator ShowCoroutine()
        {
            if (_transform != null)
                yield return _transform.TweenPositionY(_initialY, _duration)
                    .SetFrom(_initialY - _offset)
                    .SetEaseQuadOut()
                    .Yield();
        }

        protected override IEnumerator HideCoroutine()
        {
            if (_transform != null)
            {
                yield return _transform.TweenPositionY(_initialY + _offset, _duration)
                    .SetEaseQuadIn()
                    .Yield();

                _transform.position = new Vector3(_transform.position.x, _initialY, _transform.position.z);
            }
        }
    }
}
