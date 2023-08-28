using FishNet.Connection;
using FishNet.Object;
using FishNet.Transporting;
using UnityEngine;

namespace Gameplay
{
    public class ServerGameLogic : NetworkBehaviour
    {
        [SerializeField]
        private Lobby lobby;

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
            lobby.ServerHandlePlayerJoin(connection);
        }

        private void HandlePlayerLeave(NetworkConnection connection)
        {
            lobby.ServerHandlePlayerLeave(connection);

            foreach (var networkObject in connection.Objects) // todo: who's responsibility is it to clean these up?
                Despawn(networkObject);
        }
    }
}
