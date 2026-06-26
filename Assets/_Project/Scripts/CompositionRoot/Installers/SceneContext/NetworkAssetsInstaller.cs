using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public class NetworkAssetsInstaller : AdvancedMonoInstaller
    {
        [Header("AssetReferences:")]
        [SerializeField] private AssetReference _networkRunnerAssetReference;
        [SerializeField] private AssetReference _networkSceneManagerReference;
        [SerializeField] private AssetReference _networkObjectsProvider;
        
        public override void InstallBindings()
        {
            BindAsset("NetworkRunnerAssetReference", _networkRunnerAssetReference);
            BindAsset("NetworkSceneManagerReference", _networkSceneManagerReference);
            BindAsset("NetworkObjectsProviderReference", _networkObjectsProvider);
        }
    }
}