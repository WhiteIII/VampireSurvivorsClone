using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Common.Services.Factories.Base
{
    public interface INetworkObjectsCreator<in TBaseItem>
    {
        UniTask<T> Create<T>(AssetReference assetReference) where T : TBaseItem;
        UniTask<T> Create<T>(AssetReference assetReference, Vector3 position) where T : TBaseItem;
        void Despawn(TBaseItem item);
    }
}