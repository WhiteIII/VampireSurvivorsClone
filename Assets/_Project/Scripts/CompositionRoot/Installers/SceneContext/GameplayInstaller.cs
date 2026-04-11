using _Project.Scripts.CompositionRoot.EntryPoints;
using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using _Project.Scripts.Gameplay.Network.Services.Factories;
using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Implementation;
using _Project.Scripts.Gameplay.Network.Services.Factories.Implementation;
using _Project.Scripts.Gameplay.Network.Services.GameCycle;
using _Project.Scripts.Gameplay.Network.Services.InputSystem;
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
        /*[Header("OnScene:")] 
        [SerializeField] private Transform _repositoriesParent;
        
        [Header("Prefabs:")]
        [SerializeField] private AssetReference _playerPrefabAssetReference;
        [SerializeField] private AssetReference _gameLoopAssetReference;
        
        public override void InstallBindings()
        {
            BindAssets();
            BindCreators();
            BindRepositories();
            Container.Bind<IFactory<Vector3, PlayerRef, UniTask<Player>>>().To<PlayerFactory>().AsSingle();
            Container.Bind<GameLoop>().FromFactory<GameLoopFactory>().AsSingle();
            Container.BindInterfacesTo<PlayerSpawner>().AsSingle();
            Container.BindInterfacesTo<InputController>().AsSingle();
            Container.BindInterfacesTo<GameplayEntryPoint>().AsSingle();
        }

        private void BindRepositories()
        {
            BindCreatorOrRepository<PlayerRepository>();
            BindCreatorOrRepository<NetworkObjectsRepository>();
        }
        
        private void BindCreators()
        {
            BindCreatorOrRepository<PlayerCreator>();
            BindCreatorOrRepository<NetworkObjectsCreator>();
        }
        
        private void BindAssets()
        {
            Container.Bind<AssetReference>().WithId("PlayerPrefabAssetReference")
                .FromInstance(_playerPrefabAssetReference);
            Container.Bind<AssetReference>().WithId("GameLoopAssetReference")
                .FromInstance(_gameLoopAssetReference);
        }

        private void BindCreatorOrRepository<T>() where T : NetworkBehaviour =>
            Container.Bind<T>().FromFactory<CreatorAndRepositoryFactory<T>>().AsSingle();*/
    }
}