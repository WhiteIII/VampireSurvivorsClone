using System;
using _Project.Scripts.CompositionRoot.Services;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public abstract class AdvancedMonoInstaller : MonoInstaller
    {
        private AsyncDependenciesContainer _asyncDependenciesContainer;
        
        public sealed override void InstallBindings()
        {
            AsyncDependenciesContainerFactory factory = Container.Instantiate<AsyncDependenciesContainerFactory>();
            _asyncDependenciesContainer = factory.Create();
            OnInstallBindings();
        }

        public void BindAsync<TValue, TFactory>() where TFactory : IFactory<UniTask<TValue>> where TValue : class =>
            _asyncDependenciesContainer.Register<TValue, TFactory>();
        
        protected void BindAsset(string id, AssetReference instance) => 
            BindWithId<AssetReference>(id).FromInstance(instance);

        protected void BindWithId<T>(string id, T instance) => 
            BindWithId<T>(id).FromInstance(instance);

        private ConcreteBinderGeneric<T> BindWithId<T>(string id) => 
            Container.Bind<T>().WithId(id);
        
        protected ConcreteIdArgConditionCopyNonLazyBinder BindIsSingle<T>() =>
            Container.Bind<T>().AsSingle();

        protected ConcreteIdArgConditionCopyNonLazyBinder BindInterfacesToIsSingle<T>() =>
            Container.BindInterfacesTo<T>().AsSingle();

        protected ConcreteIdArgConditionCopyNonLazyBinder BindInterfacesAndSelfToIsSingle<T>() =>
            Container.BindInterfacesAndSelfTo<T>().AsSingle();

        protected abstract void OnInstallBindings();
    }
}