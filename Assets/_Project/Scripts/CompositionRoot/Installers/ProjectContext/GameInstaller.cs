using _Project.Scripts.Common.AssetsManagement;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.ProjectContext
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings() => 
            Container.Bind<LocalAssetProvider>().AsSingle();
    }
}