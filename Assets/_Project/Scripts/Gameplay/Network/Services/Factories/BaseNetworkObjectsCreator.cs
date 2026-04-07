using System;
using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Common.Services.Factories.Base;
using _Project.Scripts.Common.Services.Repositories.Base;
using _Project.Scripts.Gameplay.Network.Services.Factories.NetworkObjectProvider;
using _Project.Scripts.Gameplay.Network.Services.GameCycle;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Gameplay.Network.Services.Factories
{
    public abstract class BaseNetworkObjectsCreator<TBaseItem> : NetworkBehaviour, INetworkObjectsCreator<TBaseItem>
        where TBaseItem : NetworkBehaviour
    {
        private const uint ID_FOR_EMPTY_NETWORK_OBJECT = 10001;
        
        private LocalAssetProvider _localAssetProvider;
        private NetworkRunner _networkRunner; 
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
            _networkRunner = generalNetworkObjectsRepository.CurrentNetworkRunner;
            _networkObjectProvider = generalNetworkObjectsRepository.CurrentNetworkObjectProvider;
            _repository = repository;
            _gameLoop = gameLoop;
        }

        public T Create<T>(AssetReference assetReference) where T : TBaseItem =>
            CreateWithParameters<T>(assetReference);

        public T Create<T>(AssetReference assetReference, Vector3 position) where T : TBaseItem => 
            CreateWithParameters<T>(assetReference, position);

        public T CreateEmptyNetworkObject<T>() => 
            CreateEmptyNetworkObject<T>();

        public void Despawn(TBaseItem item)
        {
            _gameLoop.TryUnregister(item);
            if (_repository.TryGet(out TBaseItem _))
                _repository.Remove(item);
            _networkRunner.Despawn(item.Object);
        }

        protected T CreateWithParameters<T>(
            AssetReference assetReference, 
            Vector3? position = null, 
            Quaternion? rotation = null,
            PlayerRef? playerRef = null) where T : TBaseItem
        {
            if (_networkRunner.IsServer == false)
                throw new Exception("An attempt was made to create an network object that was not a server!");
            
            return _gameLoop.TryRegister(_repository.Add(
                _networkRunner.Spawn(
                    _localAssetProvider.GetAsset<GameObject>(assetReference), 
                    position, 
                    rotation, 
                    playerRef).GetComponent<T>()));
        }

        protected T CreateEmptyObjectWithParameters<T>(
            AssetReference assetReference, 
            Vector3? position = null, 
            Quaternion? rotation = null,
            PlayerRef? playerRef = null) where T : TBaseItem
        {
            if (_networkRunner.IsServer == false)
                throw new Exception("An attempt was made to create an network object that was not a server!");

            NetworkPrefabId networkPrefabId = NetworkPrefabId.FromRaw(ID_FOR_EMPTY_NETWORK_OBJECT);
            _networkObjectProvider.SetPrefabIdAndComponentType<T>(ID_FOR_EMPTY_NETWORK_OBJECT);
            
            return _gameLoop.TryRegister(
                _repository.Add(
                    _networkRunner.Spawn(
                        networkPrefabId, position, rotation, playerRef).GetComponent<T>()));
        }
    }
}