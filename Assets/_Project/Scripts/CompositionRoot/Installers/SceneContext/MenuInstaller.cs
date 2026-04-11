using _Project.Scripts.CompositionRoot.EntryPoints;
using _Project.Scripts.Gameplay.Network;
using _Project.Scripts.Gameplay.Network.Services.Factories;
using _Project.Scripts.Gameplay.Network.Services.Factories.Implementation;
using _Project.Scripts.Gameplay.Network.Services.Factories.NetworkObjectProvider;
using Fusion;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public class MenuInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<NetworkRunnerCallBacksListener>().AsSingle();

            BindFactory<NetworkRunner, NetworkRunnerFactory>();
            BindFactory<NetworkSceneManagerDefault, NetworkSceneManagerFactory>();
            BindFactory<NetworkObjectEndEmptyObjectProvider, NetworkObjectsProviderFactory>();

            Container.BindInterfacesTo<MenuEntryPoint>().AsSingle();
        }

        private void BindFactory<TType, TFactory>() where TFactory : IFactory<TType> =>
            Container.Bind<IFactory<TType>>().To<TFactory>().AsSingle();
    }
}