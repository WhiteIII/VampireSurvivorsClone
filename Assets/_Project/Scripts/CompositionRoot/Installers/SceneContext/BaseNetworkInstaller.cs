using _Project.Scripts.Common.SceneSwitcher;
using _Project.Scripts.Common.Services.Initialize;
using _Project.Scripts.CompositionRoot.EntryPoints;
using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Fusion;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public abstract class BaseNetworkInstaller : AdvancedMonoInstallerWithAsyncInjection
    {
        private GameStateSwitcher _gameStateSwitcher;
        private NetworkComponentCreationRepository _networkComponentCreationRepository;
        
        protected bool IsServer
        {
            get
            {
                if (_gameStateSwitcher.StartGameArgs.GameMode == GameMode.Host)
                    return true;
                return false;
            }
        }
        
        [Inject] private void Construct(
            GameStateSwitcher gameStateSwitcher,
            NetworkComponentCreationRepository networkComponentCreationRepository)
        {
            _gameStateSwitcher = gameStateSwitcher;
            _networkComponentCreationRepository = networkComponentCreationRepository;
        }
        
        protected void RegisterNetworkPrefab<T>() where T : NetworkBehaviour => 
            _networkComponentCreationRepository.RegisterTypeAndGetTypeId<T>();
    }
}