using _Project.Scripts.CompositionRoot.Services;
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
    public class PlayerFactory : NetworkFactory<Player, PlayerCreator>, INetworkFactory<Player, Vector3, PlayerRef, bool>
    {
        private AssetReference _playerAssetReference;
        
        [Inject] private async UniTask Construct(
            [Inject(Id = "PlayerPrefabAssetReference")] AssetReference prefabAssetReference,
            IAsyncDependenciesContainer asyncDependenciesContainer)
        {
            _playerAssetReference = prefabAssetReference;
            
            bool hasStateAuthority = await GetStateAuthorityAsync();
            if (hasStateAuthority == false)
            {
                EndInitialization();
                return;
            }
            
            PlayerCreator creator = await asyncDependenciesContainer.Resolve<PlayerCreator>();
            Initialize(creator);
            EndInitialization();
        } 
        
        public UniTask<Player> Create(Vector3 spawnPosition, PlayerRef playerRef, bool isWithInjection) => 
            Creator.Create<Player>(_playerAssetReference, isWithInjection, spawnPosition, playerRef);

        public void Despawn(Player item) => 
            Creator.Despawn(item);
    }
}