using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Common.Services.Factories.Base
{
    public interface INetworkObjectsCreator<in TBaseItem> : ILocalObjectsCreator<TBaseItem>
        where TBaseItem : NetworkBehaviour
    {
        T Create<T>(AssetReference assetReference, Vector3 position) where T : TBaseItem;
        void Despawn(TBaseItem item);
    }
}