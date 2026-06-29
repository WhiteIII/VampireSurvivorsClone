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

        protected override void OnInstallBindings()
        {
            BindIsSingle<GeneralNetworkObjectsCreator>();
            BindFactory<NetworkRunner, NetworkRunnerFactory>();
            BindFactory<NetworkSceneManagerDefault, NetworkSceneManagerFactory>();
            BindFactory<NetworkObjectEndEmptyObjectProvider, NetworkObjectsProviderFactory>();
            
            BindInterfacesAndSelfToIsSingle<AsyncDependenciesRepository>();
            BindInterfacesAndSelfToIsSingle<SpawnPositionHelper>();
            Container.Bind<NetworkRunnerCallBacksListener>()
                .FromFactory<NetworkRunnerCallBacksListenerFactory>().AsSingle();
            BindIsSingle<AsyncInitializableRepository>();
            BindIsSingle<EnemySpawnPositionHelper>();
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
            BindNetworkComponent<EnemySpawner>();
            BindNetworkComponent<GameLoop>();
            BindNetworkComponent<EnemyObjectPool>();
            BindNetworkComponent<EnemySpawnPositionHelper>();
            BindNetworkComponent<IdGenerator>();
            BindNetworkComponent<EnemyRepository>();
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

        private void BindNetworkComponent<T>() 
            where T : NetworkBehaviour
        {
            if (IsServer)
                BindAsyncFromFactory<T, NetworkComponentFactory<T>>();
            
        }

        private void BindNetworkComponentIfIsClient<T>() 
            where T : NetworkBehaviour
        {
             
        }
        
        private void BindAsyncFromFactory<TContract, TFactory>()
            where TFactory : IFactory<UniTask<TContract>>
        {
            Container.Bind<IAsyncDependence>().To<AsyncDependence<TContract>>()
                .FromFactory<LayerAboveAsyncFactory<TContract, TFactory>>().AsSingle();
        }
    }
}