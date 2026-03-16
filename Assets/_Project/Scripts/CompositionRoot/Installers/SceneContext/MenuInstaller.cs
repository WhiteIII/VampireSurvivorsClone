using _Project.Scripts.Common.Services.Factories.Base;
using _Project.Scripts.CompositionRoot.EntryPoints;
using _Project.Scripts.Gameplay.Network;
using _Project.Scripts.Gameplay.Network.Services.Factories;
using Fusion;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public class MenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<NetworkRunnerCallBacksListener>().AsSingle();

            Container.Bind<IFactory<NetworkRunner>>().To<NetworkRunnerFactory>().AsSingle();
            Container.Bind<IFactory<NetworkSceneManagerDefault>>().To<NetworkSceneManagerFactory>().AsSingle();

            Container.BindInterfacesTo<MenuEntryPoint>().AsSingle();
        }
    }
}