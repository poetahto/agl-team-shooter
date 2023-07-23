using Core;
using Cysharp.Threading.Tasks;
using Poetools.UI.Builders;
using Poetools.UI.Items;
using UniRx;
using UnityEngine;

public class PauseMenuLogic : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;

    private IVisibilityTransition _visibility;
    private bool _settingsShown;

    private void Awake()
    {
        _visibility = CommonTransitions.Fade(canvasGroup);
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

    private void HandleShowSettings()
    {
        _settingsShown = true;
        Services.PopupFactory.ShowSettings(() => _settingsShown = false);
    }
}
