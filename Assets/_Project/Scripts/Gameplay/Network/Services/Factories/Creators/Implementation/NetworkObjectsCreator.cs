using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Base;
using _Project.Scripts.Gameplay.Network.Services.GameCycle;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Fusion;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Implementation
{
    public class NetworkObjectsCreator : BaseNetworkObjectsCreator<NetworkBehaviour>
    {
        [Inject] private async void Construct(
            LocalAssetProvider localAssetProvider,
            GeneralNetworkObjectsRepository networkRunner,
            AsyncDependenciesRepository dependenciesRepository,
            GameLoop gameLoop) 
        {
            NetworkObjectsRepository repository = await dependenciesRepository.GetInstanceAsync<NetworkObjectsRepository>();
            Initialize(localAssetProvider, networkRunner, repository, gameLoop);
        }
    }
}