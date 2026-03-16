using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Common.Services.Factories.Base;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Zenject;

namespace _Project.Scripts.Common.Services.Factories.Implementation
{
    public class NetworkObjectsCreator : LocalObjectCreator<Fusion.Behaviour>
    {
        public NetworkObjectsCreator(
            GeneralNetworkObjectsRepository repository, 
            IInstantiator instantiator, 
            LocalAssetProvider localAssetProvider) : base(repository, instantiator, localAssetProvider)
        {
        }
    }
}