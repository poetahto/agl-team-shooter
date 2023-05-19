using Core;
using Poetools.UI.Builders;
using Poetools.UI.Items;
using UniRx;
using UnityEngine;

public class PauseMenuLogic : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;

    private IVisibilityTransition _visibility;

    private void Awake()
    {
        _visibility = new DefaultVisibilityTransition(canvasGroup);
        _visibility.Show();
    }

    private void Start()
    {
        new ExistingMenuBuilder()
            .Register("resume", new Button(_visibility.Hide))
            .Register("settings", new Button(Services.PopupFactory.ShowSettings))
            .Register("disconnect", new Button(Services.GameplaySystem.StopGame))
            .Build(gameObject)
            .AddTo(this);
    }
}
