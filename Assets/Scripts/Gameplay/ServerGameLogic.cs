using FishNet.Connection;
using FishNet.Transporting;

namespace Gameplay
{
    /// <summary>
    /// Manages raw incoming connections, and forwards accepted ones to the Lobby to be assigned
    /// a ConnectedPlayer instance, formally being added to the game.
    /// </summary>
    public class ServerGameLogic : GameplayNetworkBehavior
    {
        public override void OnStartServer()
        {
            base.OnStartServer();

            foreach (NetworkConnection connection in ServerManager.Clients.Values)
                HandlePlayerJoin(connection);

            ServerManager.OnRemoteConnectionState += HandleRemoteConnection;
        }

        private void HandleRemoteConnection(NetworkConnection connection, RemoteConnectionStateArgs args)
        {
            if (args.ConnectionState == RemoteConnectionState.Started)
                HandlePlayerJoin(connection);

            else if (args.ConnectionState == RemoteConnectionState.Stopped)
                HandlePlayerLeave(connection);
        }

        private void HandlePlayerJoin(NetworkConnection connection)
        {
            connection.OnLoadedStartScenes += SpawnPlayerForConnection;
        }

        private void SpawnPlayerForConnection(NetworkConnection connection, bool asServer)
        {
            connection.OnLoadedStartScenes -= SpawnPlayerForConnection;
            Lobby.ServerHandlePlayerJoin(connection);
        }

        private void HandlePlayerLeave(NetworkConnection connection)
        {
            Lobby.ServerHandlePlayerLeave(connection);

            foreach (var networkObject in connection.Objects) // todo: who's responsibility is it to clean these up?
                Despawn(networkObject);
        }
    }
}
