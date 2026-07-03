using _Project.Scripts.Common.Services.Initialize;
using _Project.Scripts.CompositionRoot.EntryPoints;
using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network;
using _Project.Scripts.Gameplay.Network.Services;
using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Base;
using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Implementation;
using _Project.Scripts.Gameplay.Network.Services.Factories.Implementation;
using _Project.Scripts.Gameplay.Network.Services.Factories.NetworkObjectProvider;
using _Project.Scripts.Gameplay.Network.Services.Factories.ObjectPool;
using _Project.Scripts.Gameplay.Network.Services.Factories.Spawners.Implementation;
using _Project.Scripts.Gameplay.Network.Services.GameCycle;
using _Project.Scripts.Gameplay.Network.Services.InputSystem;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using _Project.Scripts.Gameplay.Network.Services.Spawners;
using _Project.Scripts.Gameplay.Services;
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
        
        [Header("OnScene:")]
        [SerializeField] private NetworkTransform _networkServicesParent; 
        
        public override void InstallBindings()
        {
            BindIsSingle<GeneralNetworkObjectsCreator>();
            Container.Bind<NetworkRunner>()
                .FromFactory<GeneralNetworkObjectFactory<NetworkRunner>>().AsSingle();
            Container.Bind<NetworkSceneManagerDefault>()
                .FromFactory<GeneralNetworkObjectFactory<NetworkSceneManagerDefault>>().AsSingle();
            Container.Bind<NetworkObjectEndEmptyObjectProvider>()
                .FromFactory<GeneralNetworkObjectFactory<NetworkObjectEndEmptyObjectProvider>>().AsSingle();
            Container.Bind<NetworkRunnerCallBacksListener>()
                .FromFactory<NetworkRunnerCallBacksListenerFactory>().AsSingle();

            BindInterfacesAndSelfToIsSingle<AsyncDependenciesRepository>();
            BindInterfacesAndSelfToIsSingle<SpawnPositionHelper>();
            BindIsSingle<AsyncInitializableRepository>();
            BindIsSingle<EnemySpawnPositionHelper>();
            BindIsSingle<FusionGameStarter>();
            BindInterfacesToIsSingle<InputController>();

            BindIsSingle<NetworkCreatorForBinding>();
            BindAssets();
            BindCreators();
            BindRepositories();
            BindFactories();
            BindParents();
            BindNetworkComponent<PlayerSpawner>();
            BindNetworkComponent<EnemySpawner>();
            BindNetworkComponent<GameLoop>();
            BindNetworkComponent<EnemyObjectPool>();
            BindNetworkComponent<EnemySpawnPositionHelper>();
            BindNetworkComponent<IdGenerator>();
            BindNetworkComponent<EnemyRepository>();
            
            BindInterfacesToIsSingle<GameplayEntryPoint>();
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
        }

        private void BindCreators()
        {
            BindNetworkComponent<PlayerCreator>();
            BindNetworkComponent<NetworkObjectsCreator>();
            BindNetworkComponent<EnemyCreator>();
        }

        private void BindAssets()
        {
            BindAsset("PlayerPrefabAssetReference", _playerPrefabAssetReference);
            BindAsset("EnemyPrefabAssetReference", _enemyPrefabAssetReference);
        }

        private void BindParents()
        {
            BindWithId("NetworkServicesParent", _networkServicesParent);
        }

        private void BindNetworkComponent<T>() 
            where T : NetworkBehaviour
        {
            RegisterNetworkPrefab<T>();
            if (IsServer)
                BindAsyncFromFactory<T, NetworkComponentFactory<T>>();
        }
        
        private void BindAsyncFromFactory<TContract, TFactory>()
            where TFactory : IFactory<UniTask<TContract>>
        {
            Container.Bind<IAsyncDependence>().To<AsyncDependence<TContract>>()
                .FromFactory<LayerAboveAsyncFactory<TContract, TFactory>>().AsSingle();
        }
    }
}