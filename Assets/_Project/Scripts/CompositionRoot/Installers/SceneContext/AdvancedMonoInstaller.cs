using Fusion;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public abstract class AdvancedMonoInstaller : MonoInstaller
    {
        protected void BindAsset(string id, AssetReference instance) => 
            BindWithId<AssetReference>(id).FromInstance(instance);
        
        protected void BindAsset(string id, NetworkPrefabRef prefabRef) => 
            BindWithId<NetworkPrefabRef>(id).FromInstance(prefabRef);
        
        protected void BindWithId<T>(string id, T instance) => 
            BindWithId<T>(id).FromInstance(instance);
        
        protected ConcreteBinderGeneric<T> BindWithId<T>(string id) => 
            Container.Bind<T>().WithId(id);
        
        protected ConcreteIdArgConditionCopyNonLazyBinder BindIsSingle<T>() =>
            Container.Bind<T>().AsSingle();

        protected ConcreteIdArgConditionCopyNonLazyBinder BindInterfacesToIsSingle<T>() =>
            Container.BindInterfacesTo<T>().AsSingle();

        protected ConcreteIdArgConditionCopyNonLazyBinder BindInterfacesAndSelfToIsSingle<T>() =>
            Container.BindInterfacesAndSelfTo<T>().AsSingle();
        
        protected void BindFactory<TType, TFactory>() where TFactory : IFactory<TType> =>
            Container.Bind<IFactory<TType>>().To<TFactory>().AsSingle();
    }
}