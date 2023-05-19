using UnityEngine;

public interface IPopupFactory
{
    void ShowSettings();
    void PromptHostGame();
    void PromptConnectGame();
}

public class PopupFactory : MonoBehaviour, IPopupFactory
{
    [SerializeField]
    private GameObject settingsPrefab;

    [SerializeField]
    private GameObject hostGamePromptPrefab;

    [SerializeField]
    private GameObject connectGamePromptPrefab;

    private void Awake()
    {
        Services.PopupFactory = this;
    }

    public void ShowSettings()
    {
        Instantiate(settingsPrefab);
    }

    public void PromptHostGame()
    {
        Instantiate(hostGamePromptPrefab);
    }

    public void PromptConnectGame()
    {
        Instantiate(connectGamePromptPrefab);
    }
}
