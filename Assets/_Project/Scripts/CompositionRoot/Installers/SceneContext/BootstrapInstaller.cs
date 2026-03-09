using _Project.Scripts.Gameplay.Services.Factories.Base;
using _Project.Scripts.Gameplay.Services.Repositories;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<MonoBehaviourObjectsRepository>().AsSingle().CopyIntoAllSubContainers();
            Container.Bind<LocalObjectCreator>().AsSingle().CopyIntoAllSubContainers();
        }
    }
}