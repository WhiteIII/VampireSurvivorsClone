using System;
using _Project.Scripts.CompositionRoot.Services;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public abstract class AdvancedMonoInstaller : MonoInstaller
    {
        public sealed override void InstallBindings()
        {
            Container.Bind(typeof(IInitializable), typeof(IDisposable))
                .To<AsyncDependenciesContainer>()
                .FromFactory<AsyncDependenciesContainer, AsyncDependenciesContainerFactory>().AsSingle();
            OnInstallBindings();
        }

        protected void BindAsync<TValue, TFactory>() where TFactory : IFactory<UniTask<TValue>> where TValue : class =>
            Container.Bind().FromFactory<TValue, BindFromAsyncDependenciesContainer<TValue, TFactory>>().AsSingle();
        
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

        protected abstract void OnInstallBindings();

        private ConcreteBinderGeneric<T> BindWithId<T>(string id) => 
            Container.Bind<T>().WithId(id);
    }
}