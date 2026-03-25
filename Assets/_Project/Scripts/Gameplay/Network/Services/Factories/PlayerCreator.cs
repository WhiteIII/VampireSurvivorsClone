using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using _Project.Scripts.Gameplay.Network.Services.GameCycle;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories
{
    public class PlayerCreator : BaseNetworkObjectsCreator<Player>
    {
        public PlayerCreator(
            LocalAssetProvider localAssetProvider,
            DiContainer diContainer,
            GeneralNetworkObjectsRepository networkRunner, 
            PlayerRepository repository,
            GameLoop gameLoop) : 
            base(localAssetProvider, diContainer, networkRunner, repository, gameLoop)
        {
        }
        
        public T Create<T>(AssetReference assetReference, Vector3 position, PlayerRef playerRef) where T : Player => 
            CreateWithParameters<T>(assetReference, position, null, playerRef);
    }
}