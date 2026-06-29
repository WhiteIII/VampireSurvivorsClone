using System.Collections.Generic;
using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Common.Services.Initialize;
using _Project.Scripts.Gameplay.Network.Services.Factories.NetworkObjectProvider;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using _Project.Scripts.View.Implementation;
using _Project.Scripts.View.Services;
using _Project.Scripts.ViewModel.Implementation;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.CompositionRoot.EntryPoints
{
    public class GameplayEntryPoint : IInitializable
    {
        private readonly AsyncInitializableRepository _initializableRepository;
        private readonly UIController _uiController;
        private readonly LoadingWindowViewModel _loadingWindowViewModel;
        private readonly GeneralNetworkObjectsRepository _generalNetworkObjectsRepository;
        private readonly AssetsLoader _assetsLoader;
        private readonly IFactory<NetworkRunner> _networkRunnerFactory;
        private readonly IFactory<NetworkSceneManagerDefault> _networkSceneManagerDefaultFactory;
        private readonly IFactory<NetworkObjectEndEmptyObjectProvider> _networkObjectEndEmptyObjectProviderFactory;
        private readonly AssetReference _networkRunnerAssetReference;
        private readonly AssetReference _networkSceneManagerAssetReference;
        private readonly AssetReference _networkObjectProviderAssetReference;
        
        public GameplayEntryPoint(
            AsyncInitializableRepository initializableRepository,
            UIController uiController,
            LoadingWindowViewModel loadingWindowViewModel,
            GeneralNetworkObjectsRepository generalNetworkObjectsRepository, 
            IFactory<NetworkRunner> networkRunnerFactory,
            IFactory<NetworkSceneManagerDefault> networkSceneManagerDefaultFactory, 
            IFactory<NetworkObjectEndEmptyObjectProvider> networkObjectEndEmptyObjectProviderFactory, 
            [Inject(Id = "NetworkRunnerAssetReference")] AssetReference networkRunnerAssetReference,
            [Inject(Id = "NetworkSceneManagerReference")] AssetReference networkSceneManagerReference,
            [Inject(Id = "NetworkObjectsProviderReference")] AssetReference networkObjectsProviderRefence, 
            AssetsLoader assetsLoader)
        {
            _initializableRepository = initializableRepository;
            _uiController = uiController;
            _loadingWindowViewModel = loadingWindowViewModel;
            _generalNetworkObjectsRepository = generalNetworkObjectsRepository;
            _networkRunnerFactory = networkRunnerFactory;
            _networkSceneManagerDefaultFactory = networkSceneManagerDefaultFactory;
            _networkObjectEndEmptyObjectProviderFactory = networkObjectEndEmptyObjectProviderFactory;
            _networkRunnerAssetReference = networkRunnerAssetReference;
            _networkSceneManagerAssetReference = networkSceneManagerReference;
            _networkObjectProviderAssetReference = networkObjectsProviderRefence;
            _assetsLoader = assetsLoader;
        }

        public async void Initialize()
        {
            AddAssetsForLoading();
            CreateGeneralNetworkObjects();
            await _loadingWindowViewModel.StartLoadingAsync(GetLoadedTasks());
            await _uiController.CloseWindowAsync<LoadingWindow>();
            List<NetworkObject> list = _generalNetworkObjectsRepository.CurrentNetworkRunner.GetAllNetworkObjects();

            Debug.Log($"Count: {list.Count}");
            foreach (NetworkObject networkObject in list)
            {
                Debug.Log(networkObject);
                Debug.Log(networkObject.GetComponent<NetworkBehaviour>().GetType().FullName);
            }
        }

        private void CreateGeneralNetworkObjects()
        {
            _networkRunnerFactory.Create();
            _networkSceneManagerDefaultFactory.Create();
            _networkObjectEndEmptyObjectProviderFactory.Create();
        }
        
        private void AddAssetsForLoading()
        {
            _assetsLoader.AddAsset(_networkRunnerAssetReference);
            _assetsLoader.AddAsset(_networkSceneManagerAssetReference);
            _assetsLoader.AddAsset(_networkObjectProviderAssetReference);
        }
        
        private UniTask[] GetLoadedTasks()
        {
            List<UniTask> tasks = new();
            tasks.AddRange(_initializableRepository.GetTasks());
            tasks.AddRange(_assetsLoader.GetLoadedTaskAssets());
            return tasks.ToArray();
        }
    }
}