using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Fusion;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Base
{
    public class NetworkCreatorForBinding : NetworkObjectCreator<NetworkBehaviour>
    {
        public NetworkCreatorForBinding(
            LocalAssetProvider localAssetProvider,
            GeneralNetworkObjectsRepository generalNetworkObjectsRepository,
            IInstantiator instantiator) :
            base(localAssetProvider, generalNetworkObjectsRepository, instantiator)
        {
        }
    }
}