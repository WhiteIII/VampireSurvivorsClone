using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Common.Services.Factories.Implementation;
using _Project.Scripts.Common.Services.Repositories.Implementation;
using _Project.Scripts.CompositionRoot.Installers.SceneContext;
using _Project.Scripts.CompositionRoot.Services;

namespace _Project.Scripts.CompositionRoot.Installers.ProjectContext
{
    public class GameInstaller : AdvancedMonoInstaller
    {
        public override void InstallBindings()
        {
            BindIsSingle<GlobalRepository>();
            BindInterfacesToIsSingle<AsyncDependenciesContainer>();
            Container.Bind<LocalAssetProvider>()
                .FromFactory<FactoryWithGlobalRepository<LocalAssetProvider>>().AsSingle();
        }
    }
}