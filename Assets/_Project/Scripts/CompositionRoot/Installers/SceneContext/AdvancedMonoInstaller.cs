using Fusion;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public abstract class AdvancedMonoInstaller : MonoInstaller
    {
        protected void BindAsset(string id, AssetReference instance) => 
            Container.Bind<AssetReference>().WithId(id).FromInstance(instance);
        
        protected void BindAsset(string id, NetworkPrefabRef prefabRef) => 
            Container.Bind<NetworkPrefabRef>().WithId(id).FromInstance(prefabRef);
        
        protected void BindIsSingle<T>() =>
            Container.Bind<T>().AsSingle();

        protected void BindInterfacesToIsSingle<T>() =>
            Container.BindInterfacesTo<T>().AsSingle();

        protected void BindInterfacesAndSelfToIsSingle<T>() =>
            Container.BindInterfacesAndSelfTo<T>().AsSingle();
    }
}