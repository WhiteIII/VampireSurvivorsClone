using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories
{
    public class PlayerFactory : BaseNetworkObjectFactory<Player, PlayerCreator>, IFactory<Vector3, PlayerRef, Player>
    {
        public PlayerFactory(
            PlayerCreator playerCreator, 
            [Inject(Id = "PlayerPrefabAssetReference")]AssetReference prefabAssetReference) : 
            base(playerCreator, prefabAssetReference)
        {
        }

        public Player Create(Vector3 position, PlayerRef playerRef)
        {
            Player player = NetworkObjectsCreator.Create<Player>(PrefabAssetReference, position, playerRef);
            player.Initialize(playerRef);
            return player;
        } 
    }
}