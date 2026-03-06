using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using R3;

public class NetworkRunnerCallBacksListener : INetworkRunnerCallbacks
{
    public Observable<(NetworkRunner, PlayerRef)> OnPlayerJoinedSubject => _onPlayerJoinedSubject;
    public Observable<(NetworkRunner, PlayerRef)> OnPlayerLeftSubject => _onPlayerLeftSubject;
    public Observable<(NetworkRunner, NetworkInput)> OnInputSubject => _onInputSubject;
    
    private readonly Subject<(NetworkRunner, PlayerRef)> _onPlayerJoinedSubject = new();
    private readonly Subject<(NetworkRunner, PlayerRef)> _onPlayerLeftSubject = new();
    private readonly Subject<(NetworkRunner, NetworkInput)> _onInputSubject = new();
        
    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player) =>
        _onPlayerJoinedSubject.OnNext((runner, player));

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player) => 
        _onPlayerLeftSubject.OnNext((runner, player));
        
    public void OnInput(NetworkRunner runner, NetworkInput input) => 
        _onInputSubject.OnNext((runner, input));

    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }

    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }

    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
        
    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) {}

    public void OnConnectedToServer(NetworkRunner runner) { }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    public void OnSceneLoadDone(NetworkRunner runner) { }

    public void OnSceneLoadStart(NetworkRunner runner) { }
}