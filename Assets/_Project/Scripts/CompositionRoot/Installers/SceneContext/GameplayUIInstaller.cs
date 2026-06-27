using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.View.Base;
using _Project.Scripts.VIew.Services.Factories.Implementation;
using _Project.Scripts.ViewModel.Implementation;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public class GameplayUIInstaller : AdvancedMonoInstaller
    {
        [Header("UIPrefabs:")]
        [SerializeField] private AssetReference _enemiesBarsWindowAssetReference;
        [SerializeField] private AssetReference _enemyBarWindowAssetReference;
        
        public override void InstallBindings()
        {
            BindFactories();
            BindAssets();
            BindViewModels();
        }

        private void BindViewModels()
        {
            BindIsSingle<EnemiesBarsViewModel>();
        }
        
        private void BindFactories()
        {
            BindIsSingle<EnemyBarFactory>().WhenInjectedInto<EnemiesBarsWindowFactory>();
            BindUIFactory<EnemiesBarsWindowFactory>();
        }

        private void BindAssets()
        {
            BindAsset("EnemiesBarsWindowAssetReference", _enemiesBarsWindowAssetReference);
            BindAsset("EnemyBarAssetReference", _enemiesBarsWindowAssetReference);
        }

        private ConcreteIdArgConditionCopyNonLazyBinder BindUIFactory<T>() where T : IFactory<Window> => 
            Container
                .Bind<IFactory<Window>>()
                .FromFactory<UIFactoryLayerAboveFactory<EnemiesBarsWindowFactory>>()
                .AsSingle();
    }
}