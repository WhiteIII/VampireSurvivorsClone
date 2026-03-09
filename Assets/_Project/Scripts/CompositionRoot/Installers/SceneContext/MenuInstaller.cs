using _Project.Scripts.Gameplay.Network;
using _Project.Scripts.Gameplay.Network.Services.Factories;
using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public class MenuInstaller : MonoInstaller
    {
        [Header("Prefabs references:")]
        [SerializeField] private AssetReference _networkRunnerPrefabRef; 
        [SerializeField] private AssetReference _networkSceneManagerPrefabRef;
        
        public override void InstallBindings()
        {
            Container.Bind<NetworkRunnerCallBacksListener>().AsSingle();
            BindNetworkRunner();
            BindSceneManager();
        }

        private void BindNetworkRunner()
        {
            Container.BindIFactory<NetworkRunner, NetworkRunnerFactory>().WithFactoryArguments(_networkRunnerPrefabRef);
            Container.Bind<NetworkRunner>().FromFactory<NetworkRunnerFactory>().AsSingle();
        }

        private void BindSceneManager()
        {
            Container
                .BindIFactory<NetworkSceneManagerDefault, BaseMonoBehaviourObjectFactory<NetworkSceneManagerDefault>>()
                .WithFactoryArguments(_networkSceneManagerPrefabRef);
            Container.Bind<NetworkSceneManagerDefault>().AsSingle();
        }
    }
}