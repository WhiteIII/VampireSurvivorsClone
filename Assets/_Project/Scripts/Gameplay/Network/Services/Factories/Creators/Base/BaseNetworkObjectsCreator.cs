using System;
using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Common.Services.Factories.Base;
using _Project.Scripts.Common.Services.Repositories.Base;
using _Project.Scripts.Gameplay.Network.Services.Factories.NetworkObjectProvider;
using _Project.Scripts.Gameplay.Network.Services.GameCycle;
using _Project.Scripts.Gameplay.Network.Services.HostMigration;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Base
{
    public abstract class BaseNetworkObjectsCreator<TBaseItem> : 
        NetworkBehaviour, 
        INetworkObjectsCreator<TBaseItem>,
        IOnHostMigration
        where TBaseItem : NetworkBehaviour
    {
        private const uint ID_FOR_EMPTY_NETWORK_OBJECT = 10001;
        
        private LocalAssetProvider _localAssetProvider;
        private IRepository<TBaseItem> _repository;
        private GameLoop _gameLoop;
        private NetworkObjectEndEmptyObjectProvider _networkObjectProvider;
        
        protected void Initialize(
            LocalAssetProvider localAssetProvider,
            GeneralNetworkObjectsRepository generalNetworkObjectsRepository,
            IRepository<TBaseItem> repository,
            GameLoop gameLoop)
        {
            _localAssetProvider = localAssetProvider;
            _networkObjectProvider = generalNetworkObjectsRepository.CurrentNetworkObjectProvider;
            _repository = repository;
            _gameLoop = gameLoop;
        }

        public void OnHostMigration(GeneralNetworkObjectsRepository generalNetworkObjectsRepository) =>
            _networkObjectProvider = generalNetworkObjectsRepository.CurrentNetworkObjectProvider;

        public UniTask<T> Create<T>(AssetReference assetReference) where T : TBaseItem =>
            CreateWithParameters<T>(assetReference);

        public UniTask<T> Create<T>(AssetReference assetReference, Vector3 position) where T : TBaseItem => 
            CreateWithParameters<T>(assetReference, position);

        public UniTask<T> CreateEmptyNetworkObjectWithComponent<T>() where T : TBaseItem =>
            CreateEmptyObjectWithParameters<T>();

        public void Despawn(TBaseItem item)
        {
            _gameLoop.TryUnregister(item);
            if (_repository.TryGet(out TBaseItem _))
                _repository.Remove(item);
            Runner.Despawn(item.Object);
        }

        protected async UniTask<T> CreateWithParameters<T>(
            AssetReference assetReference, 
            Vector3? position = null, 
            Quaternion? rotation = null,
            PlayerRef? playerRef = null) where T : TBaseItem
        {
            if (Runner.IsServer == false)
                throw new Exception("An attempt was made to create an network object that was not a server!");
            
            NetworkObject spawnedObject = await Runner.SpawnAsync(
                    _localAssetProvider.GetAsset<GameObject>(assetReference), 
                    position, 
                    rotation, 
                    playerRef);
            
            return _gameLoop.TryRegister(_repository.Add(spawnedObject.GetComponent<T>()));
        }

        protected async UniTask<T> CreateEmptyObjectWithParameters<T>(
            Vector3? position = null, 
            Quaternion? rotation = null,
            PlayerRef? playerRef = null) where T : TBaseItem
        {
            if (Runner.IsServer == false)
                throw new Exception("An attempt was made to create an network object that was not a server!");

            NetworkPrefabId networkPrefabId = NetworkPrefabId.FromRaw(ID_FOR_EMPTY_NETWORK_OBJECT);
            _networkObjectProvider.SetPrefabIdAndComponentType<T>(ID_FOR_EMPTY_NETWORK_OBJECT);
            
            NetworkObject spawnedObject = await Runner.SpawnAsync(
                networkPrefabId, 
                position, 
                rotation, 
                playerRef);
            
            return _gameLoop.TryRegister(_repository.Add(spawnedObject.GetComponent<T>()));
        }
    }
}