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
    public class PlayerFactory : NetworkFactory<Player, PlayerCreator>, INetworkFactory<Player, Vector3, PlayerRef>
    {
        private NetworkPrefabRef _playerAssetReference;

        [Inject] private async void Construct(
            [Inject(Id = "PlayerPrefabAssetReference")] NetworkPrefabRef prefabAssetReference, 
            AsyncDependenciesRepository asyncDependenciesRepository)
        {
            _playerAssetReference = prefabAssetReference;
            PlayerCreator creator = await asyncDependenciesRepository.GetInstanceAsync<PlayerCreator>();
            Initialize(creator);
            EndInitialization();
        } 
        
        public async UniTask<Player> Create(Vector3 spawnPosition, PlayerRef playerRef) => 
            await Creator.Create<Player>(_playerAssetReference, spawnPosition, playerRef);

        public void Despawn(Player item) => 
            Creator.Despawn(item);
    }
}