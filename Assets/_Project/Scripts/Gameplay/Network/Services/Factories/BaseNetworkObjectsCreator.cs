using System;
using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Common.Repositories.Base;
using _Project.Scripts.Common.Services.Factories.Base;
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
        private readonly GameLoopRegistrationHelper _gameLoopRegistrationHelper;
        
        protected BaseNetworkObjectsCreator(
            LocalAssetProvider localAssetProvider,
            DiContainer diContainer,
            GeneralNetworkObjectsRepository generalNetworkObjectsRepository,
            IRepository<TBaseItem> repository)
        {
            _localAssetProvider = localAssetProvider;
            _diContainer = diContainer;
            _networkRunner = generalNetworkObjectsRepository.CurrentNetworkRunner;
            _repository = repository;
        }

        public T Create<T>(AssetReference assetReference) where T : TBaseItem =>
            CreateWithParameters<T>(assetReference);

        public T Create<T>(AssetReference assetReference, Vector3 position) where T : TBaseItem => 
            CreateWithParameters<T>(assetReference, position);

        protected T CreateWithParameters<T>(
            AssetReference assetReference, 
            Vector3? position = null, 
            Quaternion? rotation = null,
            PlayerRef? playerRef = null) where T : TBaseItem
        {
            if (_networkRunner.IsServer == false)
                throw new Exception("An attempt was made to create an network object that was not a server!");
            
            return _gameLoopRegistrationHelper.TryRegister(_repository.Add(
                _networkRunner.Spawn(
                    _localAssetProvider.GetAsset<GameObject>(assetReference), 
                    position, 
                    rotation, 
                    playerRef, 
                    (_, createdObject) => _diContainer.Inject(createdObject)).GetComponent<T>()));
        }
    }
}