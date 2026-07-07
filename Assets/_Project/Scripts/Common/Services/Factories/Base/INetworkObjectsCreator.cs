using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using R3;

namespace _Project.Scripts.Common.Services.Factories.Base
{
    public interface INetworkObjectsCreator<TBaseItem>
    {
        Observable<TBaseItem> OnSpawn { get; }
        Observable<TBaseItem> OnDespawn { get; }
        UniTask<T> Create<T>(AssetReference assetReference, bool isWithInjection) where T : TBaseItem;
        UniTask<T> Create<T>(AssetReference assetReference, bool isWithInjection, Vector3 position) where T : TBaseItem;
        void Despawn(TBaseItem item);
    }
}