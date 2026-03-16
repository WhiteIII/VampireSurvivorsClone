using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public class NetworkAssetsInstaller : MonoInstaller
    {
        [Header("AssetReferences:")]
        [SerializeField] private AssetReference _networkRunnerAssetReference;
        [SerializeField] private AssetReference _networkSceneManagerReference;
        
        public override void InstallBindings()
        {
            Container.Bind<AssetReference>().WithId("NetworkRunnerAssetReference")
                .FromInstance(_networkRunnerAssetReference);
            Container.Bind<AssetReference>().WithId("NetworkSceneManagerReference")
                .FromInstance(_networkSceneManagerReference);
        }
    }
}