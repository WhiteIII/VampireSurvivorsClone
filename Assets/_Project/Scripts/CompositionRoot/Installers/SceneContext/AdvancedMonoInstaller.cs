using System;
using _Project.Scripts.CompositionRoot.Services;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public abstract class AdvancedMonoInstaller : MonoInstaller
    {
        private IAsyncDependenciesContainer _dependenciesContainer;
        
        [Inject] private void Construct(IAsyncDependenciesContainer dependenciesContainer) => 
            _dependenciesContainer = dependenciesContainer;

        protected void BindAsync<TValue, TFactory>() where TFactory : IFactory<UniTask<TValue>> where TValue : class =>
            throw new NotImplementedException(); //_dependenciesContainer.Register();
        
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