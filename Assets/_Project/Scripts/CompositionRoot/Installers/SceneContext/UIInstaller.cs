using _Project.Scripts.Common.Services.Factories;
using _Project.Scripts.Common.Services.Factories.Implementation;
using _Project.Scripts.View.Base;
using _Project.Scripts.View.Implementation;
using _Project.Scripts.View.Services;
using _Project.Scripts.VIew.Services.Factories.Implementation;
using _Project.Scripts.View.Services.Repositrories;
using _Project.Scripts.ViewModel.Implementation;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public class UIInstaller : AdvancedMonoInstaller
    {
        [Header("AssetReferences:")]
        [SerializeField] private AssetReference _loadingWindowAssetReference;
        [SerializeField] private AssetReference _menuWindowAssetReference;
        [SerializeField] private AssetReference _enemiesBarsWindowAssetReference;
        [SerializeField] private AssetReference _enemyBarWindowAssetReference;
        
        [Header("OnScene:")]
        [SerializeField] private RectTransform _uiRootRectTransform;
        
        public override void InstallBindings()
        {
            BindAsset("LoadingWindowAssetReference", _loadingWindowAssetReference);
            BindAsset("MenuWindowAssetReference", _menuWindowAssetReference);
            BindAsset("EnemiesBarsWindowAssetReference", _enemiesBarsWindowAssetReference);
            BindAsset("EnemyBarAssetReference", _enemyBarWindowAssetReference);
            
            BindWindowFactory<LoadingWindowFactory>();
            BindWindowFactory<MenuWindowFactory>();
            Container.Bind<IFactory<EnemyBar>>().To<EnemyBarFactory>().AsSingle().WhenInjectedInto<EnemiesBarsWindow>();

            BindInterfacesAndSelfToIsSingle<MenuViewModel>();
            BindIsSingle<CreateGameOrConnectToGameViewModel>();
            BindIsSingle<LoadingWindowViewModel>();
            
            BindWindowsServices();
        }
        
        private void BindWindowsServices()
        {
            BindIsSingle<WindowsRepository>();
            BindIsSingle<WindowsCreator>();
            BindIsSingle<UIRoot>().WithArguments(_uiRootRectTransform);
            BindIsSingle<UIController>();
        }
        
        private void BindWindowFactory<T>() where T : IFactory<Window> =>
            Container.Bind<IFactory<Window>>().To<T>().AsSingle().WhenInjectedInto<UIController>();
        
        private void BindAsyncWindowFactory<TValue, TFactory>()
            where TValue : Window
            where TFactory : IFactory<UniTask<TValue>>
        {
            Container.Bind<TFactory>().AsSingle().WhenInjectedInto<AbstractOverAsyncFactory<TFactory, TValue>>();
            Container.Bind<IAbstractOverAsyncFactory<Window>>()
                .To<AbstractOverAsyncFactory<TFactory, TValue>>().AsSingle().WhenInjectedInto<UIController>();
        }
    }
}