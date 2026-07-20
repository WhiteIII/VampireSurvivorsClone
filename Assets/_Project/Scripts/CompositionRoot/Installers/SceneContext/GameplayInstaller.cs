using _Project.Scripts.Common.Services.Factories.Base;
using _Project.Scripts.Common.Services.Initialize;
using _Project.Scripts.CompositionRoot.EntryPoints;
using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network;
using _Project.Scripts.Gameplay.Network.Services;
using _Project.Scripts.Gameplay.Network.Services.BaseComponent.Local;
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
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public class GameplayInstaller : BaseNetworkInstaller
    {
        [Header("NetworkPrefabs:")] 
        [SerializeField] private AssetReference _playerPrefabAssetReference;
        [SerializeField] private AssetReference _enemyPrefabAssetReference;
        
        [Header("OnScene:")]
        [SerializeField] private NetworkTransform _networkServicesParent;
        [SerializeField] private Map _map;
        [SerializeField] private Camera _camera;
        
        public override void InstallBindings()
        {
            BindIsSingle<GeneralNetworkObjectsCreator>();
            BindIsSingle<NetworkBehavioursRepository>();
            BindIsSingle<GameLoopLocalBuffer>().WhenInjectedInto<GameLoop>();
            BindInterfacesAndSelfToIsSingle<PlayersInSessionData>();
            BindIsSingle<CameraController>().WithArguments(_camera);
            Container.Bind<Map>().FromInstance(_map).AsSingle();
            Container.Bind<NetworkRunner>()
                .FromFactory<GeneralNetworkObjectFactory<NetworkRunner>>().AsSingle();
            Container.Bind<NetworkSceneManagerDefault>()
                .FromFactory<GeneralNetworkObjectFactory<NetworkSceneManagerDefault>>().AsSingle();
            Container.Bind<NetworkObjectEndEmptyObjectProvider>()
                .FromFactory<GeneralNetworkObjectFactory<NetworkObjectEndEmptyObjectProvider>>().AsSingle();
            Container.Bind<NetworkRunnerCallBacksListener>()
                .FromFactory<NetworkRunnerCallBacksListenerFactory>().AsSingle();

            BindInterfacesAndSelfToIsSingle<SpawnPositionHelper>();
            BindIsSingle<EnemySpawnPositionHelper>();
            BindIsSingle<FusionGameStarter>();
            BindInterfacesToIsSingle<InputController>();

            BindIsSingle<NetworkCreatorForBinding>();
            BindAssets();
            BindParents();
            BindNetworkComponent<PlayerFactory>();
            BindNetworkComponent<EnemyFactory>();
            BindNetworkComponent<PlayerRepository>();
            BindNetworkComponent<PlayerCreator>();
            BindNetworkComponent<EnemyCreator>();
            BindNetworkComponent<PlayerSpawner>();
            BindNetworkComponent<EnemySpawner>();
            BindNetworkComponent<GameLoop>();
            BindNetworkComponent<EnemyObjectPool>();
            BindNetworkComponent<EnemySpawnPositionHelper>();
            BindNetworkComponent<IdGenerator>();
            BindNetworkComponent<EnemyRepository>();
            
            BindInterfacesToIsSingle<GameplayEntryPoint>();
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
                BindAsync<T, NetworkComponentFactory<T>>();
            else
                BindAsync<T, NetworkComponentFactoryIsClient<T>>();
        }
    }
}