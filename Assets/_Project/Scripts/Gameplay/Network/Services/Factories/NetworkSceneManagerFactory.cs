using _Project.Scripts.Gameplay.Network.Services.Factories;
using Fusion;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Common.Services.Factories.Base
{
    public class NetworkSceneManagerFactory : NetworkObjectFactory<NetworkSceneManagerDefault>
    {
        public NetworkSceneManagerFactory(
            [Inject(Id = "NetworkSceneManagerReference")]AssetReference assetReference, 
            GeneralNetworkObjectsCreator creator) : base(assetReference, creator)
        {
        }
    }
}