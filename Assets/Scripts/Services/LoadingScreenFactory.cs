using System;
using Core;
using Cysharp.Threading.Tasks;
using ElRaccoone.Tweens;
using Poetools.UI.Items;
using UnityEngine;

public interface ILoadingScreenFactory
{
    UniTask<IDisposable> FadeColor(Color color, float duration = 0.5f);
    UniTask<IDisposable> SlideRightColor(Color color, float duration = 0.5f);
}

public class LoadingScreenFactory : MonoBehaviour, ILoadingScreenFactory
{
    [SerializeField]
    private ColorScreenView colorScreenPrefab;

    private void Awake()
    {
        Services.LoadingScreenFactory = this;
    }

    public async UniTask<IDisposable> FadeColor(Color color, float duration = 0.5f)
    {
        ColorScreenView instance = Instantiate(colorScreenPrefab, transform);
        instance.image.color = color;
        IVisibilityTransition transition = CommonTransitions.Fade(instance.canvasGroup, duration);
        transition.Show();
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
        return new DisposableAction(() => transition.HideThenDestroy());
    }

    public async UniTask<IDisposable> SlideRightColor(Color color, float duration = 0.5f)
    {
        ColorScreenView instance = Instantiate(colorScreenPrefab, transform);
        instance.image.color = color;
        instance.rectTransform.pivot = new Vector2(1, 0.5f);
        instance.rectTransform.TweenPositionX(Screen.width, duration).SetFrom(0).SetEaseQuadOut();
        await UniTask.Delay(TimeSpan.FromSeconds(duration));
        return new DisposableAction(() => instance.rectTransform.TweenPositionX(Screen.width * 2, duration).SetEaseQuadIn());
    }
}
