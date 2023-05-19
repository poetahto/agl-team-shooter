using UnityEngine;

public interface IGameUI
{
    void ShowPauseMenu();
}

[RequireComponent(typeof(GameplayServices))]
public class GameUI : MonoBehaviour, IGameUI
{
    [SerializeField]
    private GameObject pauseMenuPrefab;

    private void Awake()
    {
        GetComponent<GameplayServices>().GameUI = this;
    }

    public void ShowPauseMenu()
    {
        Instantiate(pauseMenuPrefab);
    }
}
