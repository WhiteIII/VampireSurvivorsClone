using _Project.Scripts.Common.Services.Initialize;
using _Project.Scripts.CompositionRoot.EntryPoints;
using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network;
using _Project.Scripts.Gameplay.Network.Services;
using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Base;
using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Implementation;
using _Project.Scripts.Gameplay.Network.Services.Factories.Implementation;
using _Project.Scripts.Gameplay.Network.Services.GameCycle;
using _Project.Scripts.Gameplay.Network.Services.InputSystem;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using _Project.Scripts.Gameplay.Network.Services.Spawners;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public class GameplayInstaller : BaseNetworkInstaller
    {
        [Header("NetworkPrefabs:")] 
        [SerializeField] private NetworkPrefabRef _playerPrefabAssetReference;
        [SerializeField] private NetworkPrefabRef _enemyPrefabAssetReference;

        protected override void OnInstallBindings()
        {
            BindInterfacesAndSelfToIsSingle<AsyncDependenciesRepository>();
            BindInterfacesAndSelfToIsSingle<SpawnPositionHelper>();
            Container.Bind<NetworkRunnerCallBacksListener>()
                .FromFactory<NetworkRunnerCallBacksListenerFactory>().AsSingle();
            BindIsSingle<AsyncInitializableRepository>();
            BindAssets();
            BindInterfacesToIsSingle<InputController>();

            BindInterfacesToIsSingle<GameplayEntryPoint>();
        }

        protected override void BindIfIsServer()
        {
            BindIsSingle<NetworkCreatorForBinding>();
            BindCreators();
            BindRepositories();
            BindFactories();
            BindNetworkComponent<PlayerSpawner>();
            BindNetworkComponent<GameLoop>();
        }

        private void BindFactories()
        {
            BindNetworkComponent<PlayerFactory>();
            BindNetworkComponent<EnemyFactory>();
        }
        
        private void BindRepositories()
        {
            BindNetworkComponent<PlayerRepository>();
            BindNetworkComponent<NetworkObjectsRepository>();
            BindNetworkComponent<EnemyRepository>();
        }

        private void BindCreators()
        {
            BindNetworkComponent<PlayerCreator>();
            BindNetworkComponent<NetworkObjectsCreator>();
            BindNetworkComponent<EnemyCreator>();
        }

        private void BindAssets()
        {
            Container.Bind<NetworkPrefabRef>().WithId("PlayerPrefabAssetReference")
                .FromInstance(_playerPrefabAssetReference);
            Container.Bind<NetworkPrefabRef>().WithId("EnemyPrefabAssetReference")
                .FromInstance(_enemyPrefabAssetReference);
        }

        private void BindNetworkComponent<T>() where T : NetworkBehaviour =>
            BindAsyncFromFactory<T, NetworkComponentFactory<T>>();

        private void BindAsyncFromFactory<TContract, TFactory>()
            where TFactory : IFactory<UniTask<TContract>>
        {
            Container.Bind<IAsyncDependence>().To<AsyncDependence<TContract>>()
                .FromFactory<LayerAboveAsyncFactory<TContract, TFactory>>().AsSingle();
        }
    }
}