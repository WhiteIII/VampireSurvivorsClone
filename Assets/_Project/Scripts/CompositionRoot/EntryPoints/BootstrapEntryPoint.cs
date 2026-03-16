using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Common.SceneSwitcher;
using _Project.Scripts.View.Implementation;
using _Project.Scripts.View.Services;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.CompositionRoot.EntryPoints
{
    public class BootstrapEntryPoint : IInitializable
    {
        private readonly GameStateSwitcher _gameStateSwitcher;
        private readonly AssetsLoader _assetLoader;
        private readonly AssetReference _loadingWindowAssetReference;
        private readonly UIController _uiController;
        
        public BootstrapEntryPoint(
            GameStateSwitcher gameStateSwitcher, 
            AssetsLoader assetLoader, 
            [Inject(Id = "LoadingWindowAssetReference")] AssetReference loadingWindowAssetReference, 
            UIController uiController)
        {
            _gameStateSwitcher = gameStateSwitcher;
            _assetLoader = assetLoader;
            _loadingWindowAssetReference = loadingWindowAssetReference;
            _uiController = uiController;
        }

        public async void Initialize()
        { 
            _assetLoader.AddAsset(_loadingWindowAssetReference);
            foreach (UniTask task in _assetLoader.GetLoadedTaskAssets())
                await task;
            await _uiController.CreateAndOpenWindowAsync<LoadingWindow>();
            await _gameStateSwitcher.GoToMenu();
        }
    }
}