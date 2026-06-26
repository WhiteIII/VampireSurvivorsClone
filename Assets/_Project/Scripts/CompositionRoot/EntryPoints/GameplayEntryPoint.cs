using System.Collections.Generic;
using _Project.Scripts.Common.Services.Initialize;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using _Project.Scripts.View.Implementation;
using _Project.Scripts.View.Services;
using _Project.Scripts.ViewModel.Implementation;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.CompositionRoot.EntryPoints
{
    public class GameplayEntryPoint : IInitializable
    {
        private readonly AsyncInitializableRepository _initializableRepository;
        private readonly UIController _uiController;
        private readonly LoadingWindowViewModel _loadingWindowViewModel;
        private readonly GeneralNetworkObjectsRepository _generalNetworkObjectsRepository;
        
        public GameplayEntryPoint(
            AsyncInitializableRepository initializableRepository,
            UIController uiController,
            LoadingWindowViewModel loadingWindowViewModel,
            GeneralNetworkObjectsRepository generalNetworkObjectsRepository)
        {
            _initializableRepository = initializableRepository;
            _uiController = uiController;
            _loadingWindowViewModel = loadingWindowViewModel;
            _generalNetworkObjectsRepository = generalNetworkObjectsRepository;
        }

        public async void Initialize()
        {
            await _loadingWindowViewModel.StartLoadingAsync(_initializableRepository.GetTasks());
            await _uiController.CloseWindowAsync<LoadingWindow>();
            List<NetworkObject> list = _generalNetworkObjectsRepository.CurrentNetworkRunner.GetAllNetworkObjects();

            Debug.Log($"Count: {list.Count}");
            foreach (NetworkObject networkObject in list)
            {
                Debug.Log(networkObject);
                Debug.Log(networkObject.GetComponent<NetworkBehaviour>().GetType().FullName);
            }
        }
    }
}