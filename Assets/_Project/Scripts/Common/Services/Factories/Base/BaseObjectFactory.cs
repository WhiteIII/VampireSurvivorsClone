using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Common.Services.Factories.Base
{
    public abstract class BaseObjectFactory<T> : IFactory<T>
        where T : MonoBehaviour
    {
        private readonly AssetReference _assetReference;
        private readonly ILocalObjectsCreator<T> _creator;

        public BaseObjectFactory(
            AssetReference assetReference,
            ILocalObjectsCreator<T> creator)
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