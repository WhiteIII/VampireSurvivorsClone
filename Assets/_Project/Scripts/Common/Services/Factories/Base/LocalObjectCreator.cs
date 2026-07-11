using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Common.Services.Repositories.Base;
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
        private readonly DiContainer _container;
        
        protected LocalObjectCreator(
            IRepository<TBaseItem> repository, 
            LocalAssetProvider localAssetProvider,
            DiContainer container)
        {
            _repository = repository;
            _localAssetProvider = localAssetProvider;
            _container = container;
        }

        public T Create<T>(AssetReference assetReference) where T : TBaseItem => 
            _repository.Add(
                _container.InstantiatePrefab(
                    _localAssetProvider.GetAsset<GameObject>(assetReference)).GetComponent<T>());

        public TValue Create<TValue, TParameter>(AssetReference assetReference, TParameter parameter) 
            where TValue : TBaseItem
        {
            DiContainer subContainer = _container.CreateSubContainer();
            subContainer.Bind<TParameter>().FromInstance(parameter).WhenInjectedInto<TValue>();
            return _repository.Add(
                subContainer.InstantiatePrefab(
                    _localAssetProvider.GetAsset<GameObject>(assetReference)).GetComponent<TValue>());
        }
        
        public T CreateComponentOnGameObject<T>() where T : TBaseItem => 
            _repository.Add(_container.InstantiateComponentOnNewGameObject<T>());
    }
}