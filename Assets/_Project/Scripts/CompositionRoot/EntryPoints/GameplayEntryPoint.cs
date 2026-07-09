using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Common.Services.Initialize;
using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network.Services;
using _Project.Scripts.Gameplay.Network.Services.Factories.Spawners.Implementation;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using _Project.Scripts.View.Implementation;
using _Project.Scripts.View.Services;
using _Project.Scripts.ViewModel.Implementation;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.CompositionRoot.EntryPoints
{
    public class GameplayEntryPoint : BaseNetworkSceneEntryPoint, IInitializable
    {
        private readonly AsyncInitializableRepository _initializableRepository;
        private readonly UIController _uiController;
        private readonly LoadingWindowViewModel _loadingWindowViewModel;
        private readonly AssetsLoader _assetsLoader;
        private readonly FusionGameStarter _gameStarter;
        private readonly AssetReference _enemiesBarsWindowAssetReference;
        private readonly AssetReference _enemyBarAssetReference;
        private readonly AssetReference _playerAssetReference;
        private readonly AssetReference _enemyAssetReference;
        private readonly NetworkBehavioursRepository _networkBehavioursRepository;
        private readonly AsyncDependenciesRepository _networkServicesRepository;
        
        public GameplayEntryPoint(
            NetworkRunner networkRunner,
            AsyncInitializableRepository initializableRepository,
            UIController uiController,
            LoadingWindowViewModel loadingWindowViewModel,
            GeneralNetworkObjectsRepository generalNetworkObjectsRepository, 
            [Inject(Id = "EnemiesBarsWindowAssetReference")] AssetReference enemiesBarsWindowAssetReference,
            [Inject(Id = "EnemyBarAssetReference")] AssetReference enemyBarAssetReference,
            AssetsLoader assetsLoader,
            FusionGameStarter fusionGameStarter,
            NetworkBehavioursRepository networkBehavioursRepository,
            [Inject(Id = "PlayerPrefabAssetReference")]AssetReference playerAssetReference,
            [Inject(Id = "EnemyPrefabAssetReference")]AssetReference enemyAssetReference, 
            AsyncDependenciesRepository networkServicesRepository) : base(networkRunner)
        {
            _initializableRepository = initializableRepository;
            _uiController = uiController;
            _loadingWindowViewModel = loadingWindowViewModel;
            _enemiesBarsWindowAssetReference = enemiesBarsWindowAssetReference;
            _enemyBarAssetReference = enemyBarAssetReference;
            _assetsLoader = assetsLoader;
            _gameStarter = fusionGameStarter;
            _networkBehavioursRepository = networkBehavioursRepository;
            _playerAssetReference = playerAssetReference;
            _enemyAssetReference = enemyAssetReference;
            _networkServicesRepository = networkServicesRepository;
        }

        public override async void Initialize()
        {
            AddAssetsForLoading();
            _loadingWindowViewModel.StartMultiStageLoading(GetTotalTasksCount());
            await _loadingWindowViewModel.WaitLoadingForMultiStageLoadingAsync(_assetsLoader.GetLoadedTaskAssets());
            await _loadingWindowViewModel.WaitLoadingForMultiStageLoadingAsync(_gameStarter.StartGameAsync());
            await _loadingWindowViewModel.WaitLoadingForMultiStageLoadingAsync(_networkBehavioursRepository.InitializeAsync());
            await _loadingWindowViewModel.WaitLoadingForMultiStageLoadingAsync(_initializableRepository.GetTasks());
            await _uiController.CreateAndOpenWindowAsync<EnemiesBarsWindow>();
            await _uiController.CloseWindowAsync<LoadingWindow>();
            _loadingWindowViewModel.ResetLoadingProgress();
            await IfIsHost();
        }

        private async UniTask IfIsHost()
        {
            if (IsServer == false)
                return;

            EnemySpawner enemySpawner = await _networkServicesRepository.GetInstanceAsync<EnemySpawner>();
            enemySpawner.Enable();
        }

        private void OnGameEnd()
        {
            _assetsLoader.UnloadAssets(
                _enemiesBarsWindowAssetReference, 
                _enemyBarAssetReference, 
                _playerAssetReference,
                _enemyAssetReference);
        }
        
        private void AddAssetsForLoading()
        {
            _assetsLoader.AddAsset(_enemiesBarsWindowAssetReference);
            _assetsLoader.AddAsset(_enemyBarAssetReference);
            _assetsLoader.AddAsset(_playerAssetReference);
            _assetsLoader.AddAsset(_enemyAssetReference);
        }

        private int GetTotalTasksCount()
        {
            int startGameUnit = 1;
            int networkBehavioursRepositoryUnit = 1;
            return _assetsLoader.NotLoadedAssetsCount + 
                   _initializableRepository.Count + 
                   startGameUnit + 
                   networkBehavioursRepositoryUnit;
        }
    }
}