using Fusion;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories
{
    public abstract class BaseNetworkObjectFactory<TValue, TCreator> :  IFactory
        where TValue : NetworkBehaviour
        where TCreator : BaseNetworkObjectsCreator<TValue>
    {
        protected readonly TCreator NetworkObjectsCreator;
        protected readonly AssetReference PrefabAssetReference;

        protected BaseNetworkObjectFactory(
            TCreator networkObjectsCreator, 
            AssetReference prefabAssetReference)
        {
            NetworkObjectsCreator = networkObjectsCreator;
            PrefabAssetReference = prefabAssetReference;
        }
    }
}