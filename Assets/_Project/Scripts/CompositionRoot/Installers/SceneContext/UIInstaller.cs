using _Project.Scripts.Common.Services.Factories.Implementation;
using _Project.Scripts.View.Base;
using _Project.Scripts.View.Services;
using _Project.Scripts.VIew.Services.Factories.Implementation;
using _Project.Scripts.View.Services.Repositrories;
using _Project.Scripts.ViewModel.Implementation;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public class UIInstaller : MonoInstaller
    {
        [Header("AssetReferences:")]
        [SerializeField] private AssetReference _loadingWindowAssetReference;
        [SerializeField] private AssetReference _menuWindowAssetReference;
        
        [Header("OnScene:")]
        [SerializeField] private RectTransform _uiRootRectTransform;
        
        public override void InstallBindings()
        {
            BindAssetReferences(_loadingWindowAssetReference, "LoadingWindowAssetReference");
            BindAssetReferences(_menuWindowAssetReference, "MenuWindowAssetReference");
            
            BindWindowFactory<LoadingWindowFactory>();
            BindWindowFactory<MenuWindowFactory>();
            
            Container.BindInterfacesAndSelfTo<MenuViewModel>().AsSingle();
            Container.Bind<CreateGameOrConnectToGameViewModel>().AsSingle();
            Container.Bind<LoadingWindowViewModel>().AsSingle();
            
            BindWindowsServices();
        }

        private void BindAssetReferences(AssetReference assetReference, string id) => 
            Container.Bind<AssetReference>().WithId(id).FromInstance(assetReference);
        
        private void BindWindowsServices()
        {
            Container.Bind<WindowsRepository>().AsSingle();
            Container.Bind<WindowsCreator>().AsSingle();
            Container.Bind<UIRoot>().AsSingle().WithArguments(_uiRootRectTransform);
            Container.Bind<UIController>().AsSingle();
        }
        
        private void BindWindowFactory<T>() where T : IFactory<Window> =>
            Container.Bind<IFactory<Window>>().To<T>().AsSingle();
    }
}