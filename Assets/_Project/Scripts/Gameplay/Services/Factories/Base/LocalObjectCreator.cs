using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Gameplay.Services.Repositories;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Services.Factories.Base
{
    public class LocalObjectCreator
    {
        private readonly MonoBehaviourObjectsRepository _monoBehaviourObjectsRepository;
        private readonly LocalAssetProvider _localAssetProvider;
        private readonly IInstantiator _instantiator;

        public LocalObjectCreator(
            MonoBehaviourObjectsRepository monoBehaviourObjectsRepository, 
            IInstantiator instantiator, 
            LocalAssetProvider localAssetProvider)
        {
            _monoBehaviourObjectsRepository = monoBehaviourObjectsRepository;
            _instantiator = instantiator;
            _localAssetProvider = localAssetProvider;
        }

        public T Create<T>(AssetReference assetReference) where T : MonoBehaviour => 
            _monoBehaviourObjectsRepository.Add(
                _instantiator.InstantiatePrefab(
                    _localAssetProvider.GetAsset<GameObject>(assetReference)).GetComponent<T>());
    }
}
