using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Base;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Implementation
{
    public class PlayerCreator : NetworkLayerAboveObjectCreator<Player>
    {
        [Inject] private async void Construct(
            AsyncDependenciesRepository dependenciesRepository,
            IInstantiator instantiator)
        {
            PlayerRepository repository = await dependenciesRepository.GetInstanceAsync<PlayerRepository>();
            await Initialize(repository, dependenciesRepository, instantiator);
            EndInitialization();
        }

        public async UniTask<T> Create<T>(NetworkPrefabRef networkPrefabRef, Vector3 position, PlayerRef playerRef)
            where T : Player
        {
            T player = await CreateWithParameters<T>(networkPrefabRef, position, null, playerRef);
            player.Initialize(playerRef);
            return player;
        } 
    }
}