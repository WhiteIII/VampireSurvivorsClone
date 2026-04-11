using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Implementation;
using Fusion;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Implementation
{
    public class NetworkSceneManagerFactory : GeneralNetworkObjectFactory<NetworkSceneManagerDefault>
    {
        public NetworkSceneManagerFactory(
            [Inject(Id = "NetworkSceneManagerReference")]AssetReference assetReference, 
            GeneralNetworkObjectsCreator creator) : base(assetReference, creator)
        {
        }
    }
}