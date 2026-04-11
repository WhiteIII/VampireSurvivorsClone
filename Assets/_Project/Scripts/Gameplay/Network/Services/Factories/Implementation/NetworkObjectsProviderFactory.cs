using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Implementation;
using _Project.Scripts.Gameplay.Network.Services.Factories.NetworkObjectProvider;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Implementation
{
    public class NetworkObjectsProviderFactory : GeneralNetworkObjectFactory<NetworkObjectEndEmptyObjectProvider>
    {
        public NetworkObjectsProviderFactory(
            [Inject(Id = "NetworkObjectsProviderReference")]AssetReference assetReference, 
            GeneralNetworkObjectsCreator creator) : base(assetReference, creator)
        {
        }
    }
}