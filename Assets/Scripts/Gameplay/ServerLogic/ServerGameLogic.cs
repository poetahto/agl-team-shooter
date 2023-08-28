using FishNet.Connection;
using FishNet.Object;
using FishNet.Transporting;
using UnityEngine;

namespace Gameplay
{
    public class ServerGameLogic : NetworkBehaviour
    {
        [SerializeField]
        private LoadoutSystem loadoutSystem;

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
            print($"{connection.ClientId} finished loading start scenes!");
            loadoutSystem.ServerHandlePlayerJoin(connection);

            NetworkObject loadoutPrefab = loadoutSystem.ServerGetLoadoutPrefab(connection);
            NetworkObject playerInstance = Instantiate(loadoutPrefab);
            Spawn(playerInstance, connection);
        }

        private void HandlePlayerLeave(NetworkConnection connection)
        {
            loadoutSystem.ServerHandlePlayerLeave(connection);

            foreach (var networkObject in connection.Objects)
            {
                Despawn(networkObject);
            }
        }
    }
}
