using Core;
using Core.Networking;
using Cysharp.Threading.Tasks;
using FishNet.Object;
using Gameplay;
using Poetools.UI.Builders;
using Poetools.UI.Items;
using UniRx;
using UnityEngine;

public class PauseMenuLogic : GameplayBehavior
{
    [SerializeField]
    private CanvasGroup canvasGroup;

    private IVisibilityTransition _visibility;
    private bool _settingsShown;

    private void Awake()
    {
        _visibility = CommonTransitions.Fade(canvasGroup);
        _visibility.VisibilityChanged += HandleVisibilityChanged;
    }

    private void OnDestroy()
    {
        _visibility.VisibilityChanged -= HandleVisibilityChanged;
    }

    private void Start()
    {
        new ExistingMenuBuilder()
            .Register("resume", new Button(_visibility.Hide))
            .Register("settings", new Button(HandleShowSettings))
            .Register("disconnect", new Button(() => Services.GameplayRunner.StopGame().Forget()))
            .Build(gameObject)
            .AddTo(this);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_settingsShown)
            _visibility.Toggle();
    }

    private void HandleVisibilityChanged(bool isVisible)
    {
        // todo: this sucks
        foreach (NetworkObject localObj in LocalConnection.Objects)
        {
            OwnerOnly controllers = localObj.GetComponentInChildren<OwnerOnly>(true);

            if (controllers != null)
                controllers.gameObject.SetActive(!isVisible);
        }
    }

    private void HandleShowSettings()
    {
        _settingsShown = true;
        Services.PopupFactory.ShowSettings(() => _settingsShown = false);
    }
}
