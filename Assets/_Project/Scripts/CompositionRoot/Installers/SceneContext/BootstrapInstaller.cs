using _Project.Scripts.Common.Services.Factories.Base;
using _Project.Scripts.Gameplay.Services.Repositories;
using _Project.Scripts.VIew.Services.Repositrories;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public class BootstrapInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<WindowsCreator>().AsSingle().CopyIntoAllSubContainers();
            Container.BindInterfacesAndSelfTo<WindowsRepository>().AsSingle().CopyIntoAllSubContainers();
            
            Container.Bind<LocalObjectCreator<MonoBehaviour>>().AsSingle().CopyIntoAllSubContainers();
            Container.Bind<MonoBehaviourObjectRepository>().AsSingle().CopyIntoAllSubContainers();
        }
    }
}