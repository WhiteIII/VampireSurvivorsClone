using _Project.Scripts.Common.Repositories.Base;
using _Project.Scripts.Common.Services.Factories.Base;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories
{
    public class BaseMonoBehaviourObjectFactory<T> : IFactory<T>
        where T : MonoBehaviour
    {
        private readonly AssetReference _assetReference;
        private readonly LocalObjectCreator<MonoBehaviour> _creator;

        public BaseMonoBehaviourObjectFactory(
            AssetReference assetReference,
            LocalObjectCreator<MonoBehaviour> creator)
        {
            _assetReference = assetReference;
            _creator = creator;
        }

        public virtual T Create() =>
            CreateByCreator();
        
        protected T CreateByCreator() => 
            _creator.Create<T>(_assetReference);
    }
}