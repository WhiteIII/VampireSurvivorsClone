using _Project.Scripts.Common.Services.Initialize;
using _Project.Scripts.CompositionRoot.EntryPoints;
using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network;
using _Project.Scripts.Gameplay.Network.Services;
using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Base;
using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Implementation;
using _Project.Scripts.Gameplay.Network.Services.Factories.Implementation;
using _Project.Scripts.Gameplay.Network.Services.GameCycle;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using _Project.Scripts.Gameplay.Network.Services.Spawners;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public class GameplayInstaller : MonoInstaller
    {
        [Header("NetworkPrefabs:")]
        [SerializeField] private NetworkPrefabRef _playerPrefabAssetReference;
        
        public override void InstallBindings()
        {
            BindInterfacesAndSelfToIsSingle<AsyncDependenciesRepository>();
            BindInterfacesAndSelfToIsSingle<SpawnPositionHelper>();
            Container.Bind<NetworkRunnerCallBacksListener>()
                .FromFactory<NetworkRunnerCallBacksListenerFactory>().AsSingle();
            BindIsSingle<AsyncInitializableRepository>();
            BindNetworkComponent<PlayerSpawner>();
            BindNetworkComponent<GameLoop>();
            BindIsSingle<NetworkCreatorForBinding>();
            BindNetworkComponent<PlayerFactory>();
            BindAssets();
            BindCreators();
            BindRepositories();
            //Container.BindInterfacesTo<InputController>().AsSingle();

            BindInterfacesToIsSingle<GameplayEntryPoint>();
        }

        private void BindRepositories()
        {
            BindNetworkComponent<PlayerRepository>();
            BindNetworkComponent<NetworkObjectsRepository>();
        }
        
        private void BindCreators()
        {
            BindNetworkComponent<PlayerCreator>();
            BindNetworkComponent<NetworkObjectsCreator>();
        }
        
        private void BindAssets()
        {
            Container.Bind<NetworkPrefabRef>().WithId("PlayerPrefabAssetReference")
                .FromInstance(_playerPrefabAssetReference);
        }
        
        private void BindNetworkComponent<T>() where T : NetworkBehaviour => 
            BindAsyncFromFactory<T, NetworkComponentFactory<T>>();
        
        private void BindAsyncFromFactory<TContract, TFactory>() 
            where TFactory : IFactory<UniTask<TContract>>
        {
            Container.Bind<IAsyncDependence>().To<AsyncDependence<TContract>>()
                .FromFactory<LayerAboveAsyncFactory<TContract, TFactory>>().AsSingle();
        } 
        
        private void BindIsSingle<T>() => 
            Container.Bind<T>().AsSingle();
        
        private void BindInterfacesToIsSingle<T>() => 
            Container.BindInterfacesTo<T>().AsSingle();
        
        private void BindInterfacesAndSelfToIsSingle<T>() => 
            Container.BindInterfacesAndSelfTo<T>().AsSingle();
    }
}