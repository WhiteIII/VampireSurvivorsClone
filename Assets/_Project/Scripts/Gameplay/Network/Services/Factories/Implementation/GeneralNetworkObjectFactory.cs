using _Project.Scripts.Common.Services.Factories.Base;
using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Implementation;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Implementation
{
    public abstract class GeneralNetworkObjectFactory<T> : BaseObjectFactory<T>
        where T : Fusion.Behaviour
    {
        protected GeneralNetworkObjectFactory(
            AssetReference assetReference, 
            GeneralNetworkObjectsCreator creator) : base(assetReference, creator)
        {
        }
    }
}