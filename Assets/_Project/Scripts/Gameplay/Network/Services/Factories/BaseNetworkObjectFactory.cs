using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories
{
    public abstract class BaseNetworkObjectFactory : IFactory
    {
        protected readonly AssetReference PrefabAssetReference;

        protected BaseNetworkObjectFactory(AssetReference prefabAssetReference) =>
            PrefabAssetReference = prefabAssetReference;
    }
}