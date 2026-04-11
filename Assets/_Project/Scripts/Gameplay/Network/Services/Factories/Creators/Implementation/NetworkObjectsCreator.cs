using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Base;
using _Project.Scripts.Gameplay.Network.Services.GameCycle;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Fusion;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Implementation
{
    public class NetworkObjectsCreator : BaseNetworkObjectsCreator<NetworkBehaviour>
    {
        [Inject] private void Construct(
            LocalAssetProvider localAssetProvider,
            GeneralNetworkObjectsRepository networkRunner,
            NetworkObjectsRepository repository, 
            GameLoop gameLoop) 
        {
            Initialize(localAssetProvider, networkRunner, repository, gameLoop);
        }
    }
}