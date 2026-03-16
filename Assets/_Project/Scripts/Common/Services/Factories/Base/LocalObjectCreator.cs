using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Common.Repositories.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Common.Services.Factories.Base
{
    public class LocalObjectCreator<TBaseItem> : ILocalObjectsCreator<TBaseItem>
        where TBaseItem : MonoBehaviour
    {
        private readonly IRepository<TBaseItem> _repository;
        private readonly LocalAssetProvider _localAssetProvider;
        private readonly IInstantiator _instantiator;

        public LocalObjectCreator(
            IRepository<TBaseItem> repository, 
            IInstantiator instantiator, 
            LocalAssetProvider localAssetProvider)
        {
            _repository = repository;
            _instantiator = instantiator;
            _localAssetProvider = localAssetProvider;
        }

        public T Create<T>(AssetReference assetReference) where T : TBaseItem => 
            _repository.Add(
                _instantiator.InstantiatePrefab(
                    _localAssetProvider.GetAsset<GameObject>(assetReference)).GetComponent<T>());
    }
}