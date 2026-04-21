using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Common.Services.Factories.Implementation;
using _Project.Scripts.Common.Services.Repositories.Implementation;
using _Project.Scripts.Gameplay.Network;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.ProjectContext
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GlobalRepository>().AsSingle();
            Container.Bind<LocalAssetProvider>()
                .FromFactory<FactoryWithGlobalRepository<LocalAssetProvider>>().AsSingle();
        }
    }
}