using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Common.Services.Factories.Base;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories
{
    public class NetworkObjectsCreator : ILocalObjectsCreator<NetworkBehaviour>
    {
        private readonly LocalAssetProvider _localAssetProvider;
        private readonly DiContainer _diContainer;
        private readonly GeneralNetworkObjectsRepository _generalNetworkObjectsRepository;
        private readonly NetworkObjectRepository _repository;

        public NetworkObjectsCreator(
            LocalAssetProvider localAssetProvider,
            DiContainer diContainer,
            GeneralNetworkObjectsRepository generalNetworkObjectsRepository,
            NetworkObjectRepository repository)
        {
            _localAssetProvider = localAssetProvider;
            _diContainer = diContainer;
            _generalNetworkObjectsRepository = generalNetworkObjectsRepository;
            _repository = repository;
        }

        public T Create<T>(AssetReference assetReference) where T : NetworkBehaviour
        {
            T createdObject = _repository.Add(
                _generalNetworkObjectsRepository.CurrentNetworkRunner.Spawn(
                    _localAssetProvider.GetAsset<GameObject>(assetReference)).GetComponent<T>());
            _diContainer.Inject(createdObject);
            return createdObject;
        }
    }
}