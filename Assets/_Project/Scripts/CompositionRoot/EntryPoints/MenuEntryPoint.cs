using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Common.SceneSwitcher;
using _Project.Scripts.Gameplay.Network.Services.Factories.NetworkObjectProvider;
using _Project.Scripts.View.Implementation;
using _Project.Scripts.View.Services;
using _Project.Scripts.ViewModel.Implementation;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.CompositionRoot.EntryPoints
{
    public class MenuEntryPoint : IInitializable
    {
        private readonly UIController _uiController;
        private readonly LoadingWindowViewModel _loadingWindowViewModel;
        private readonly AssetsLoader _assetsLoader;
        private readonly AssetReference _networkSceneManagerReference;
        private readonly AssetReference _networkRunnerAssetReference;
        private readonly AssetReference _menuWindowAssetReference;
        private readonly AssetReference _networkObjectsProviderAssetReference;
        private readonly MenuViewModel _menuViewModel;
        private readonly GameStateSwitcher _gameStateSwitcher;
        private readonly IFactory<NetworkRunner> _networkRunnerFactory;
        private readonly IFactory<NetworkSceneManagerDefault> _networkSceneManagerFactory;
        private readonly IFactory<NetworkObjectEndEmptyObjectProvider> _networkObjectProviderFactory;
        
        public MenuEntryPoint(
            UIController uiController,
            LoadingWindowViewModel loadingWindowViewModel,
            AssetsLoader assetsLoader, 
            [Inject(Id = "MenuWindowAssetReference")] AssetReference menuWindowAssetReference, 
            MenuViewModel menuViewModel, 
            GameStateSwitcher gameStateSwitcher, 
            IFactory<NetworkRunner> networkRunnerFactory, 
            IFactory<NetworkSceneManagerDefault> networkSceneManagerFactory, 
            [Inject(Id = "NetworkRunnerAssetReference")] AssetReference networkRunnerAssetReference,
            [Inject(Id = "NetworkSceneManagerReference")] AssetReference networkSceneManagerReference,
            [Inject(Id = "NetworkObjectsProviderReference")] AssetReference networkObjectsProviderRefence, 
            IFactory<NetworkObjectEndEmptyObjectProvider> networkObjectProviderFactory)
        {
            _uiController = uiController;
            _loadingWindowViewModel = loadingWindowViewModel;
            _assetsLoader = assetsLoader;
            _menuWindowAssetReference = menuWindowAssetReference;
            _menuViewModel = menuViewModel;
            _gameStateSwitcher = gameStateSwitcher;
            _networkRunnerFactory = networkRunnerFactory;
            _networkSceneManagerFactory = networkSceneManagerFactory;
            _networkRunnerAssetReference = networkRunnerAssetReference;
            _networkSceneManagerReference = networkSceneManagerReference;
            _networkObjectsProviderAssetReference = networkObjectsProviderRefence;
            _networkObjectProviderFactory = networkObjectProviderFactory;
        }

        public async void Initialize()
        {
            AddAssetToAssetLoader();
            _menuViewModel.SetOnToGameplayMethod(OnMoveToGameplay);
            await _loadingWindowViewModel.StartLoadingAsync(_assetsLoader.GetLoadedTaskAssets());
            CreateNetworkObjects();
            await _uiController.CloseWindowAsync<LoadingWindow>();
            await _uiController.CreateAndOpenWindowAsync<MenuWindow>();
        }

        private async UniTask OnMoveToGameplay(StartGameArgs startGameArgs)
        {
            await _uiController.DestroyAndCloseWindowsAsync(typeof(MenuWindow));
            await _uiController.OpenWindowAsync<LoadingWindow>();
            _assetsLoader.UnloadAssets(_menuWindowAssetReference);
            await _gameStateSwitcher.GoToGameplay(startGameArgs);
        }

        private void CreateNetworkObjects()
        {
            _networkRunnerFactory.Create();
            _networkSceneManagerFactory.Create();
            _networkObjectProviderFactory.Create();
        }

        private void AddAssetToAssetLoader()
        {
            _assetsLoader.AddAsset(_menuWindowAssetReference);
            _assetsLoader.AddAsset(_networkRunnerAssetReference);
            _assetsLoader.AddAsset(_networkSceneManagerReference);
            _assetsLoader.AddAsset(_networkObjectsProviderAssetReference);
        }
    }
}