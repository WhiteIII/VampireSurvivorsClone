using System;
using System.Collections.Generic;
using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Common.Services.Initialize;
using _Project.Scripts.Gameplay.Network.Services;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using _Project.Scripts.View.Implementation;
using _Project.Scripts.View.Services;
using _Project.Scripts.ViewModel.Implementation;
using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.CompositionRoot.EntryPoints
{
    public class GameplayEntryPoint : IInitializable, IDisposable
    {
        private readonly AsyncInitializableRepository _initializableRepository;
        private readonly UIController _uiController;
        private readonly LoadingWindowViewModel _loadingWindowViewModel;
        private readonly GeneralNetworkObjectsRepository _generalNetworkObjectsRepository;
        private readonly AssetsLoader _assetsLoader;
        private readonly FusionGameStarter _gameStarter;
        private readonly AssetReference _enemiesBarsWindowAssetReference;
        private readonly AssetReference _enemyBarAssetReference;
        
        public GameplayEntryPoint(
            AsyncInitializableRepository initializableRepository,
            UIController uiController,
            LoadingWindowViewModel loadingWindowViewModel,
            GeneralNetworkObjectsRepository generalNetworkObjectsRepository, 
            [Inject(Id = "EnemiesBarsWindowAssetReference")] AssetReference enemiesBarsWindowAssetReference,
            [Inject(Id = "EnemyBarAssetReference")] AssetReference enemyBarAssetReference,
            AssetsLoader assetsLoader,
            FusionGameStarter fusionGameStarter)
        {
            _initializableRepository = initializableRepository;
            _uiController = uiController;
            _loadingWindowViewModel = loadingWindowViewModel;
            _generalNetworkObjectsRepository = generalNetworkObjectsRepository;
            _enemiesBarsWindowAssetReference = enemiesBarsWindowAssetReference;
            _enemyBarAssetReference = enemyBarAssetReference;
            _assetsLoader = assetsLoader;
            _gameStarter = fusionGameStarter;
        }

        public async void Initialize()
        {
            AddAssetsForLoading();
            _loadingWindowViewModel.StartMultiStageLoading(GetTotalTasksCount());
            await _loadingWindowViewModel.WaitLoadingForMultiStageLoadingAsync(_assetsLoader.GetLoadedTaskAssets());
            await _loadingWindowViewModel.WaitLoadingForMultiStageLoadingAsync(_gameStarter.StartGameAsync());
            await _loadingWindowViewModel.WaitLoadingForMultiStageLoadingAsync(_initializableRepository.GetTasks());
            //await _uiController.CreateAndOpenWindowAsync<>();
            await _uiController.CloseWindowAsync<LoadingWindow>();
            _loadingWindowViewModel.ResetLoadingProgress();
            List<NetworkObject> list = _generalNetworkObjectsRepository.CurrentNetworkRunner.GetAllNetworkObjects();

            Debug.Log($"Count: {list.Count}");
            foreach (NetworkObject networkObject in list)
            {
                Debug.Log(networkObject);
                Debug.Log(networkObject.GetComponent<NetworkBehaviour>().GetType().FullName);
            }
        }

        public void Dispose() => 
            _assetsLoader.UnloadAssets(_enemiesBarsWindowAssetReference, _enemyBarAssetReference);

        private void AddAssetsForLoading()
        {
            _assetsLoader.AddAsset(_enemiesBarsWindowAssetReference);
            _assetsLoader.AddAsset(_enemyBarAssetReference);
        }

        private int GetTotalTasksCount()
        {
            int startGameUnit = 1;
            return 2 + _initializableRepository.Count + startGameUnit;
            // TODO в assetsLoader свойство NotLoadedAssetsCount отображает количество лишних ассетов!
        }
    }
}