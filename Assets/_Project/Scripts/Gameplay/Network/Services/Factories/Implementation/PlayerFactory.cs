using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using _Project.Scripts.Gameplay.Network.Services.Factories.Base;
using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Implementation;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Implementation
{
    public class PlayerFactory : BaseNetworkObjectFactory, IFactory<Vector3, PlayerRef, UniTask<Player>>
    {
        private readonly PlayerCreator _playerCreator;
        
        public PlayerFactory(
            PlayerCreator playerCreator, 
            [Inject(Id = "PlayerPrefabAssetReference")]AssetReference prefabAssetReference) : 
            base(prefabAssetReference)
        {
            _playerCreator = playerCreator;
        }

        public async UniTask<Player> Create(Vector3 position, PlayerRef playerRef)
        {
            Player player = await _playerCreator.Create<Player>(PrefabAssetReference, position, playerRef);
            player.Initialize(playerRef);
            return player;
        }
    }
}