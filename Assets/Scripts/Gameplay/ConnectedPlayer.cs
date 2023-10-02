using System;
using FishNet;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UniRx;

/// <summary>
/// A player that is sending and receiving information from a game server.
/// There is always one instance of this component for each connected player.
/// </summary>
public class ConnectedPlayer : NetworkBehaviour
{
    [SyncVar(OnChange = nameof(HandleNameChange))]
    public string syncedPlayerName = "Guest";

    [SyncVar(OnChange = nameof(HandleLoadoutChange))]
    public Loadout syncedLoadout = Loadout.Testing;

    [SyncVar(OnChange = nameof(HandleTeamChange))]
    public int syncedTeamId;

    [SyncVar(OnChange = nameof(HandlePlayerStateChange))]
    public PlayerState syncedPlayerState = PlayerState.Alive;

    private Subject<string> _onNameChanged;
    private Subject<Loadout> _onLoadoutChanged;
    private Subject<int> _onTeamChanged;
    private Subject<PlayerState> _onPlayerStateChanged;

    public IObservable<string> ObserveNameChanged() => _onNameChanged;
    public IObservable<Loadout> ObserveLoadoutChanged() => _onLoadoutChanged;
    public IObservable<int> ObserveTeamChanged() => _onTeamChanged;
    public IObservable<PlayerState> ObservePlayerStateChanged() => _onPlayerStateChanged;

    [ServerRpc]
    public void Rpc_ServerChangeTeam(int newTeam)
    {
        syncedTeamId = newTeam;
    }

    private void Awake()
    {
        _onNameChanged = new Subject<string>();
        _onLoadoutChanged = new Subject<Loadout>();
        _onTeamChanged = new Subject<int>();
        _onPlayerStateChanged = new Subject<PlayerState>();
    }

    private void HandleNameChange(string previous, string current, bool asServer)
    {
        if (InstanceFinder.IsHost && !asServer)
            return;

        _onNameChanged.OnNext(current);
    }

    private void HandleLoadoutChange(Loadout previous, Loadout current, bool asServer)
    {
        if (InstanceFinder.IsHost && !asServer)
            return;

        _onLoadoutChanged.OnNext(current);
    }

    private void HandleTeamChange(int previous, int current, bool asServer)
    {
        if (InstanceFinder.IsHost && !asServer)
            return;

        _onTeamChanged.OnNext(current);
    }

    private void HandlePlayerStateChange(PlayerState previous, PlayerState current, bool asServer)
    {
        if (InstanceFinder.IsHost && !asServer)
            return;

        _onPlayerStateChanged.OnNext(current);
    }
}
