using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Common.Services.Repositories.Base;
using Cysharp.Threading.Tasks;
using Fusion;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Repositories
{
    public class NetworkBehavioursRepository : IRepository<NetworkBehaviour>, IInitializable
    {
        private readonly List<NetworkBehaviour> _list = new();

        private readonly NetworkRunner _networkRunner;
        private readonly FusionGameStarter _gameStarter;

        public int Count => _list.Count;

        public UniTask InitialisationTask
        {
            get
            {
                if (_gameStarter.GameStarted)
                    return UniTask.CompletedTask;
                return UniTask.WaitWhile(() => _gameStarter.GameStarted == false);
            }
        }

        public NetworkBehavioursRepository(NetworkRunner networkRunner, FusionGameStarter gameStarter)
        {
            _networkRunner = networkRunner;
            _gameStarter = gameStarter;
        }

        public async void Initialize()
        {
            await InitialisationTask;
            if (_networkRunner.IsServer)
                return;
            List<NetworkObject> networkObjects = _networkRunner.GetAllNetworkObjects();
            foreach (NetworkObject networkObject in networkObjects)
                _list.AddRange(networkObject.NetworkedBehaviours);
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

        public T Add<T>(T item) 
            where T : NetworkBehaviour
        {
            _list.Add(item);
            return item;
        }

        public void Remove(NetworkBehaviour item) => 
            _list.Remove(item);

        public IEnumerator<NetworkBehaviour> GetEnumerator() => 
            _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => 
            GetEnumerator();
    }
}