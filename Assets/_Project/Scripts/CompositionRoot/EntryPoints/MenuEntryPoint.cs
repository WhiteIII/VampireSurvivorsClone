using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Common.SceneSwitcher;
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
        private readonly AssetReference _menuWindowAssetReference;
        private readonly MenuViewModel _menuViewModel;
        private readonly GameStateSwitcher _gameStateSwitcher;

        public MenuEntryPoint(
            UIController uiController,
            LoadingWindowViewModel loadingWindowViewModel,
            AssetsLoader assetsLoader, 
            [Inject(Id = "MenuWindowAssetReference")] AssetReference menuWindowAssetReference, 
            MenuViewModel menuViewModel, 
            GameStateSwitcher gameStateSwitcher)
        {
            _uiController = uiController;
            _loadingWindowViewModel = loadingWindowViewModel;
            _assetsLoader = assetsLoader;
            _menuWindowAssetReference = menuWindowAssetReference;
            _menuViewModel = menuViewModel;
            _gameStateSwitcher = gameStateSwitcher;
        }

        public async void Initialize()
        {
            AddAssetToAssetLoader();
            _menuViewModel.SetOnToGameplayMethod(OnMoveToGameplay);
            await _loadingWindowViewModel.StartLoadingAsync(_assetsLoader.GetLoadedTaskAssets());
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

        private void AddAssetToAssetLoader()
        {
            _assetsLoader.AddAsset(_menuWindowAssetReference);
        }
    }
}