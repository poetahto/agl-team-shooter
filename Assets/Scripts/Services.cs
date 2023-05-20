using System;
using UnityEngine;

public static class Services
{
    public static IGameplaySystem GameplaySystem { get; set; }
    public static IPopupFactory PopupFactory { get; set; }
    public static ILoadingScreenFactory LoadingScreenFactory { get; set; }
}
