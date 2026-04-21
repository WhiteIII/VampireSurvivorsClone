using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Base
{
    public abstract class BaseNetworkObjectFactory : INetworkFactory
    {
        protected readonly AssetReference PrefabAssetReference;

        protected BaseNetworkObjectFactory(AssetReference prefabAssetReference) =>
            PrefabAssetReference = prefabAssetReference;
    }
}