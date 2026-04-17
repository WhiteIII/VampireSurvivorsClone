using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Base;
using _Project.Scripts.Gameplay.Network.Services.GameCycle;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Implementation
{
    public class PlayerCreator : BaseNetworkObjectsCreator<Player>
    {
        [Inject] private async void Construct(
            LocalAssetProvider localAssetProvider,
            GeneralNetworkObjectsRepository networkRunner, 
            AsyncDependenciesRepository dependenciesRepository,
            GameLoop gameLoop)
        {
            PlayerRepository repository = await dependenciesRepository.GetInstanceAsync<PlayerRepository>();
            Initialize(localAssetProvider, networkRunner, repository, gameLoop);
        }
        
        public UniTask<T> Create<T>(AssetReference assetReference, Vector3 position, PlayerRef playerRef) where T : Player => 
            CreateWithParameters<T>(assetReference, position, null, playerRef);
    }
}