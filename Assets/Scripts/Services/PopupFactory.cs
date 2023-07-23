using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class PopupFactory : MonoBehaviour
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

    public void ShowSettings(Action onClose = null)
    {
        Instantiate(settingsPrefab).OnDestroyAsObservable().Subscribe(_ => onClose?.Invoke());
    }

    public void PromptHostGame(Action onClose = null)
    {
        Instantiate(hostGamePromptPrefab).OnDestroyAsObservable().Subscribe(_ => onClose?.Invoke());
    }

    public void PromptConnectGame(Action onClose = null)
    {
        Instantiate(connectGamePromptPrefab).OnDestroyAsObservable().Subscribe(_ => onClose?.Invoke());
    }
}
