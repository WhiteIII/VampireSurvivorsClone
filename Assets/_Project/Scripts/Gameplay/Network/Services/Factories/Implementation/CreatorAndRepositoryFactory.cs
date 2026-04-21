using _Project.Scripts.Gameplay.Network.Services.Factories.Base;
using _Project.Scripts.Gameplay.Network.Services.Factories.NetworkObjectProvider;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Cysharp.Threading.Tasks;
using Fusion;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Implementation
{
    public class CreatorAndRepositoryFactory<T> : INetworkFactory<T>
        where T : NetworkBehaviour
    {
        private const uint CREATOR_ID = 101;
        
        private readonly NetworkRunner _networkRunner;
        private readonly NetworkObjectEndEmptyObjectProvider _networkObjectEndEmptyObjectProvider;
        
        public CreatorAndRepositoryFactory(GeneralNetworkObjectsRepository repository)
        {
            _networkRunner = repository.CurrentNetworkRunner;
            _networkObjectEndEmptyObjectProvider = repository.CurrentNetworkObjectProvider;
        }

        public async UniTask<T> Create()
        {
            NetworkPrefabId networkPrefabId = NetworkPrefabId.FromRaw(CREATOR_ID);
            _networkObjectEndEmptyObjectProvider.SetPrefabIdAndComponentType<T>(CREATOR_ID);
            NetworkObject spawnedObject = await _networkRunner.SpawnAsync(networkPrefabId);
            return spawnedObject.GetComponent<T>();
        }

        public void Despawn(T item) => 
            _networkRunner.Despawn(item.Object);
    }
}