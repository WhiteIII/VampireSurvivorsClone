using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Common.Services.Initialize;
using Cysharp.Threading.Tasks;
using Fusion;

namespace _Project.Scripts.Gameplay.Network.Services.Repositories
{
    public class NetworkBehavioursRepository : IAsyncInitializable, IEnumerable<NetworkBehaviour>
    {
        private readonly List<NetworkBehaviour> _list = new();

        private readonly NetworkRunner _networkRunner;

        private bool _isActive;
        
        public UniTask InitialisationTask
        {
            get
            {
                if (_isActive)
                    return UniTask.CompletedTask;
                return UniTask.WaitWhile(() => _isActive == false);
            }
        }

        public NetworkBehavioursRepository(NetworkRunner networkRunner) =>
            _networkRunner = networkRunner;

        public async UniTask InitializeAsync()
        {
            List<NetworkObject> networkObjects;
            while (TryGetAllNetworkObjects(out networkObjects) == false)
                await UniTask.Yield();

            foreach (NetworkObject networkObject in networkObjects)
                _list.AddRange(networkObject.NetworkedBehaviours);
            _isActive = true;
        }
        
        public bool TryGet<T>(out T item) where T : NetworkBehaviour
        {
            item = null;
            foreach (NetworkBehaviour networkBehaviour in _list)
            {
                if (networkBehaviour is T concreteNetworkBehaviour)
                {
                    item = concreteNetworkBehaviour;
                    return true;
                }
            }
            return false;
        }

        private bool TryGetAllNetworkObjects(out List<NetworkObject> networkObjects)
        {
            networkObjects  = _networkRunner.GetAllNetworkObjects();
            if (networkObjects.Count == 0)
                return false;
            return true;
        }

        public IEnumerator<NetworkBehaviour> GetEnumerator() => 
            _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => 
            GetEnumerator();
    }
}