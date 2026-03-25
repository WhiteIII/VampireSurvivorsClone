using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Gameplay.Network.Services.GameCycle;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Fusion;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories
{
    public class NetworkObjectsCreator : BaseNetworkObjectsCreator<NetworkBehaviour>
    {
        public NetworkObjectsCreator(
            LocalAssetProvider localAssetProvider,
            DiContainer diContainer,
            GeneralNetworkObjectsRepository networkRunner,
            NetworkObjectsRepository repository, 
            GameLoop gameLoop) : 
            base(localAssetProvider, diContainer, networkRunner, repository, gameLoop)
        {
        }
    }
}