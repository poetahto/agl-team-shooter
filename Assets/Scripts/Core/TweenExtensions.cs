using System.Threading;
using ElRaccoone.Tweens;
using ElRaccoone.Tweens.Core;
using UnityEngine;

namespace Core
{
    public static class TweenExtensions
    {
        public static Tween<T> WithCancellation<T>(this Tween<T> tween, CancellationToken token)
        {
            token.Register(tween.Cancel);
            return tween;
        }
        
        public static Tween<float> TweenFromOffsetX(this Component component, float offset, float duration)
        {
            float x = component.transform.position.x;
            return component.TweenPositionX(x, duration).SetFrom(x + offset);
        }

        public static Tween<float> TweenOffsetX(this Component component, float offset, float duration)
        {
            float x = component.transform.position.x;
            return component.TweenPositionX(x + offset, duration);
        }

        public static Tween<float> TweenFromOffsetY(this Component component, float offset, float duration)
        {
            float y = component.transform.position.y;
            return component.TweenPositionY(y, duration).SetFrom(y + offset);
        }

        public static Tween<float> TweenOffsetY(this Component component, float offset, float duration)
        {
            float y = component.transform.position.y;
            return component.TweenPositionY(y + offset, duration);
        }
    }
}