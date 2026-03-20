using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Common.SceneSwitcher;
using _Project.Scripts.Common.Services.Factories.Implementation;
using _Project.Scripts.CompositionRoot.EntryPoints;
using _Project.Scripts.Gameplay.Network.Services.Factories;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using _Project.Scripts.ViewModel.Implementation;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<AssetsLoader>().AsSingle();
            Container.Bind<GeneralNetworkObjectsRepository>().AsSingle();
            Container.Bind<GeneralNetworkObjectsCreator>().AsSingle();
            Container.Bind<GameStateSwitcher>().AsSingle();
            Container.BindInterfacesTo<BootstrapEntryPoint>().AsSingle();
        }
    }
}