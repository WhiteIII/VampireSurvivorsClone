using System;
using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Common.Services.Factories.Base;
using _Project.Scripts.Common.Services.Repositories.Base;
using _Project.Scripts.Gameplay.Network.Services.GameCycle;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories
{
    public abstract class BaseNetworkObjectsCreator<TBaseItem> : INetworkObjectsCreator<TBaseItem>
        where TBaseItem : NetworkBehaviour
    {
        private readonly LocalAssetProvider _localAssetProvider;
        private readonly DiContainer _diContainer;
        private readonly NetworkRunner _networkRunner; 
        private readonly IRepository<TBaseItem> _repository;
        private readonly GameLoop _gameLoop;
        
        protected BaseNetworkObjectsCreator(
            LocalAssetProvider localAssetProvider,
            DiContainer diContainer,
            GeneralNetworkObjectsRepository generalNetworkObjectsRepository,
            IRepository<TBaseItem> repository,
            GameLoop gameLoop)
        {
            _localAssetProvider = localAssetProvider;
            _diContainer = diContainer;
            _networkRunner = generalNetworkObjectsRepository.CurrentNetworkRunner;
            _repository = repository;
            _gameLoop = gameLoop;
        }

        public T Create<T>(AssetReference assetReference) where T : TBaseItem =>
            CreateWithParameters<T>(assetReference);

        public T Create<T>(AssetReference assetReference, Vector3 position) where T : TBaseItem => 
            CreateWithParameters<T>(assetReference, position);

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
                    playerRef, 
                    (_, createdObject) => _diContainer.Inject(createdObject)).GetComponent<T>()));
        }
    }
}