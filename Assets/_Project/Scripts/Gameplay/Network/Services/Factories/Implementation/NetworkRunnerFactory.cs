using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Implementation;
using Fusion;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Implementation
{
    public class NetworkRunnerFactory : GeneralNetworkObjectFactory<NetworkRunner>
    {
        public NetworkRunnerFactory(
            [Inject(Id = "NetworkRunnerAssetReference")]AssetReference networkRunnerPrefab, 
            GeneralNetworkObjectsCreator creator) : base(networkRunnerPrefab, creator)
        {
        }
    }
}