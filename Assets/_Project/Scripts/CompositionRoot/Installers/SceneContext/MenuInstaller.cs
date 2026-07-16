using _Project.Scripts.CompositionRoot.EntryPoints;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public class MenuInstaller : AdvancedMonoInstaller
    {
        protected override void OnInstallBindings()
        {
            Container.BindInterfacesTo<MenuEntryPoint>().AsSingle();
        }
    }
}