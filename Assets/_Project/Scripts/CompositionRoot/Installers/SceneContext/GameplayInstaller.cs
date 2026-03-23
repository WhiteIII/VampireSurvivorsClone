using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using _Project.Scripts.Gameplay.Network.Services.Factories;
using _Project.Scripts.Gameplay.Network.Services.InputSystem;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using _Project.Scripts.Gameplay.Network.Services.Spawners;
using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public class GameplayInstaller : MonoInstaller
    {
        [Header("Prefabs:")]
        [SerializeField] private AssetReference _playerPrefabAssetReference;
        
        public override void InstallBindings()
        {
            Container.Bind<AssetReference>().WithId("PlayerPrefabAssetReference")
                .FromInstance(_playerPrefabAssetReference);
            Container.Bind<PlayerRepository>().AsSingle();
            Container.Bind<NetworkObjectsRepository>().AsSingle();
            Container.Bind<NetworkObjectsRepository>().AsSingle();
            Container.Bind<PlayerCreator>().AsSingle();
            Container.Bind<NetworkObjectsCreator>().AsSingle();
            Container.Bind<IFactory<Vector3, PlayerRef, Player>>().To<PlayerFactory>().AsSingle();
            Container.BindInterfacesTo<PlayerSpawner>().AsSingle();
            Container.BindInterfacesTo<InputController>().AsSingle();
        }
    }
}