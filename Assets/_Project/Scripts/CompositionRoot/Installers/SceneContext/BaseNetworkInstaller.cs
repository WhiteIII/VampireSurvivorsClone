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

        private NetworkObjectEndEmptyObjectProvider NetworkObjectProvider =>
            _generalNetworkObjectsRepository.CurrentNetworkObjectProvider;  
        private NetworkRunner NetworkRunner => _generalNetworkObjectsRepository.CurrentNetworkRunner; 
        
        [Inject] private void Construct(GeneralNetworkObjectsRepository generalNetworkObjectsRepository) => 
            _generalNetworkObjectsRepository = generalNetworkObjectsRepository;
        
        public sealed override void InstallBindings()
        { 
            NetworkObjectProvider.SetInstantiator(Container);
            NetworkObjectProvider.SetNetworkServicesCreationHelper(_networkServicesCreationHelper);
            
            OnInstallBindings();
            if (NetworkRunner.IsServer)
                BindIfIsServer();
        }
        
        protected abstract void OnInstallBindings();

        protected abstract void BindIfIsServer();
    }
}