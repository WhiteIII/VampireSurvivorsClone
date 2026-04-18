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
    public class PlayerFactory : NetworkFactory<Player, PlayerCreator>, IFactory<Vector3, PlayerRef, UniTask<Player>>
    {
        private AssetReference _playerAssetReference;
        
        [Inject] private void Construct([Inject(Id = "PlayerPrefabAssetReference")]AssetReference prefabAssetReference) => 
            _playerAssetReference = prefabAssetReference;
        
        public async UniTask<Player> Create(Vector3 spawnPosition, PlayerRef playerRef) => 
            await Creator.Create<Player>(_playerAssetReference, spawnPosition, playerRef);
    }
}