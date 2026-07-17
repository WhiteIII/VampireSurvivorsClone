using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Base;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Implementation
{
    public class PlayerCreator : NetworkLayerAboveObjectCreator<Player>
    {
        [Inject] private async UniTask Construct(
            IAsyncDependenciesContainer dependenciesContainer,
            IInstantiator instantiator)
        {
            bool hasStateAuthority = await GetStateAuthorityAsync();
            if (hasStateAuthority == false)
            {
                await Initialize(null, dependenciesContainer, instantiator);
                EndInitialization();
                return;
            }
            
            PlayerRepository repository = await dependenciesContainer.Resolve<PlayerRepository>();
            await Initialize(repository, dependenciesContainer, instantiator);
            EndInitialization();
        }

        public async UniTask<T> Create<T>(AssetReference assetReference, bool isWithInjection, Vector3 spawnPosition, PlayerRef playerRef)
            where T : Player
        {
            T player = await CreateWithParameters<T>(
                assetReference,
                isWithInjection,
                spawnPosition,
                null,
                playerRef);
            player.Initialize(playerRef);
            return player;
        }
    }
}