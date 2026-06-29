using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Common.SceneSwitcher;
using _Project.Scripts.CompositionRoot.EntryPoints;
using _Project.Scripts.Gameplay.Network.Services.Repositories;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public class BootstrapInstaller : AdvancedMonoInstaller
    {
        public override void InstallBindings()
        {
            BindIsSingle<AssetsLoader>();
            BindIsSingle<GeneralNetworkObjectsRepository>();
            BindIsSingle<GameStateSwitcher>();
            BindInterfacesToIsSingle<BootstrapEntryPoint>();
        }
    }
}