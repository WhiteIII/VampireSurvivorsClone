using _Project.Scripts.Gameplay.Network.Services.Factories.NetworkObjectProvider;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Fusion;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories
{
    public class CreatorsFactory<T> : BaseNetworkObjectFactory, IFactory<T>
        where T : NetworkBehaviour
    {
        private const uint CREATOR_ID = 101;
        
        private readonly NetworkRunner _networkRunner;
        private readonly NetworkObjectEndEmptyObjectProvider _networkObjectEndEmptyObjectProvider;
        
        public CreatorsFactory(
            AssetReference prefabAssetReference, 
            GeneralNetworkObjectsRepository repository) : base(prefabAssetReference)
        {
            _networkRunner = repository.CurrentNetworkRunner;
            _networkObjectEndEmptyObjectProvider = repository.CurrentNetworkObjectProvider;
        }

        public T Create()
        {
            NetworkPrefabId networkPrefabId = NetworkPrefabId.FromRaw(CREATOR_ID);
            _networkObjectEndEmptyObjectProvider.SetPrefabIdAndComponentType<T>(CREATOR_ID);
            return _networkRunner.Spawn(networkPrefabId).GetComponent<T>();
        }
    }
}