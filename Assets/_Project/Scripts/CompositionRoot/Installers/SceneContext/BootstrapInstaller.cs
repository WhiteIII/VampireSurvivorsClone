using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Common.SceneSwitcher;
using _Project.Scripts.CompositionRoot.EntryPoints;
using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network.Services.Repositories;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public class BootstrapInstaller : AdvancedMonoInstaller
    {
        protected override void OnInstallBindings()
        {
            BindIsSingle<AssetsLoader>();
            BindIsSingle<GeneralNetworkObjectsRepository>();
            BindIsSingle<GameStateSwitcher>();
            BindIsSingle<NetworkComponentCreationRepository>();
            BindInterfacesToIsSingle<BootstrapEntryPoint>();
            BindInterfacesToIsSingle<AsyncDependenciesContainersFromManyScenes>();
        }
    }
}