using _Project.Scripts.Common.Services.Factories.Base;
using _Project.Scripts.Common.Services.Factories.Implementation;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Gameplay.Network.Services.Factories
{
    public class NetworkObjectFactory<T> : BaseObjectFactory<T>
        where T : Fusion.Behaviour
    {
        public NetworkObjectFactory(
            AssetReference assetReference, 
            GeneralNetworkObjectsCreator creator) : base(assetReference, creator)
        {
        }
    }
}