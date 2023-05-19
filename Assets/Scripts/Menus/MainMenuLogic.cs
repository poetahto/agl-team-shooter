using Poetools.UI;
using Poetools.UI.Builders;
using Poetools.UI.Items;
using UniRx;
using UnityEngine;

public class MainMenuLogic : MonoBehaviour
{
    private void Start()
    {
        IPopupFactory popups = Services.PopupFactory;

        new ExistingMenuBuilder()
            .Register("host", new Button(popups.PromptHostGame))
            .Register("connect", new Button(popups.PromptConnectGame))
            .Register("settings", new Button(popups.ShowSettings))
            .Register("quit", CommonMenuItems.QuitGameButton())
            .Build(gameObject)
            .AddTo(this);
    }
}
