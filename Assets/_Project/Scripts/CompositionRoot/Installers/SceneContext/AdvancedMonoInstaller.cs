using System;
using System.Collections.Generic;
using _Project.Scripts.CompositionRoot.Services;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public abstract class AdvancedMonoInstaller : MonoInstaller
    {
        private IAsyncDependenciesContainer _dependenciesContainer;
        private readonly List<Type> _asyncDependenciesTypes = new();
        
        [Inject] private void Construct(IAsyncDependenciesContainer dependenciesContainer) => 
            _dependenciesContainer = dependenciesContainer;

        private void OnDestroy()
        {
            foreach (Type type in _asyncDependenciesTypes)
                _dependenciesContainer.Unregister(type);
            _asyncDependenciesTypes.Clear();
        }

        protected void BindAsync<TValue, TFactory>() where TFactory : IFactory<UniTask<TValue>> where TValue : class
        {
            //_dependenciesContainer.Register<T>();  
            _asyncDependenciesTypes.Add(typeof(TValue));
        }
        
        protected void BindAsset(string id, AssetReference instance) => 
            BindWithId<AssetReference>(id).FromInstance(instance);

        protected void BindWithId<T>(string id, T instance) => 
            BindWithId<T>(id).FromInstance(instance);

        protected ConcreteIdArgConditionCopyNonLazyBinder BindIsSingle<T>() =>
            Container.Bind<T>().AsSingle();

        protected ConcreteIdArgConditionCopyNonLazyBinder BindInterfacesToIsSingle<T>() =>
            Container.BindInterfacesTo<T>().AsSingle();

        protected ConcreteIdArgConditionCopyNonLazyBinder BindInterfacesAndSelfToIsSingle<T>() =>
            Container.BindInterfacesAndSelfTo<T>().AsSingle();

        private ConcreteBinderGeneric<T> BindWithId<T>(string id) => 
            Container.Bind<T>().WithId(id);
    }
}