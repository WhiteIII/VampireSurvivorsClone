using System.Collections.Generic;
using _Project.Scripts.Gameplay.Network.Services.Factories;
using _Project.Scripts.Gameplay.Network.Services.Factories.NetworkObjectProvider;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public abstract class BaseNetworkInstaller : AdvancedMonoInstaller
    {
        [Header("General:")]
        [SerializeField] private NetworkServicesCreationHelper _networkServicesCreationHelper;
        
        private GeneralNetworkObjectsRepository _generalNetworkObjectsRepository;

        protected bool IsServer => _generalNetworkObjectsRepository.CurrentNetworkRunner.IsServer;    
        
        private NetworkObjectEndEmptyObjectProvider NetworkObjectProvider =>
            _generalNetworkObjectsRepository.CurrentNetworkObjectProvider;  
        
        [Inject] private void Construct(GeneralNetworkObjectsRepository generalNetworkObjectsRepository) => 
            _generalNetworkObjectsRepository = generalNetworkObjectsRepository;
        
        public sealed override void InstallBindings()
        {
            List<NetworkObject> list = _generalNetworkObjectsRepository.CurrentNetworkRunner.GetAllNetworkObjects();

            Debug.Log($"Count: {list.Count}");
            foreach (NetworkObject networkObject in list)
            {
                Debug.Log(networkObject);
                Debug.Log(networkObject.GetComponent<NetworkBehaviour>().GetType().FullName);
            }
            
            NetworkObjectProvider.SetInstantiator(Container);
            NetworkObjectProvider.SetNetworkServicesCreationHelper(_networkServicesCreationHelper);
            
            OnInstallBindings();
            if (IsServer)
                BindIfIsServer();
        }
        
        protected abstract void OnInstallBindings();

        protected abstract void BindIfIsServer();
    }
}