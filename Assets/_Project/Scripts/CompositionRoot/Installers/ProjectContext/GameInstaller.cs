using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Common.Services.Factories.Implementation;
using _Project.Scripts.Common.Services.Repositories.Implementation;
using _Project.Scripts.CompositionRoot.Installers.SceneContext;

namespace _Project.Scripts.CompositionRoot.Installers.ProjectContext
{
    public class GameInstaller : AdvancedMonoInstaller
    {
        public override void InstallBindings()
        {
            BindIsSingle<GlobalRepository>();
            Container.Bind<LocalAssetProvider>()
                .FromFactory<FactoryWithGlobalRepository<LocalAssetProvider>>().AsSingle();
        }
    }
}