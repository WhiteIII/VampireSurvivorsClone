using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories
{
    public class PlayerFactory : BaseNetworkObjectFactory<Player>, IFactory<Vector3, PlayerRef, Player>
    {
        private readonly PlayerCreator _playerCreator;
        
        public PlayerFactory(
            PlayerCreator playerCreator, 
            [Inject(Id = "PlayerPrefabAssetReference")]AssetReference prefabAssetReference) : 
            base(prefabAssetReference)
        {
            _playerCreator = playerCreator;
        }

        public Player Create(Vector3 position, PlayerRef playerRef)
        {
            Player player = _playerCreator.Create<Player>(PrefabAssetReference, position, playerRef);
            player.Initialize(playerRef);
            return player;
        } 
    }
}