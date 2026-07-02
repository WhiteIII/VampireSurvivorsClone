using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Common.Services.Factories.Base
{
    public interface ILocalObjectsCreator<in TBaseItem>
        where TBaseItem : MonoBehaviour
    {
        T Create<T>(AssetReference assetReference) where T : TBaseItem;
        T CreateComponentOnGameObject<T>() where T : TBaseItem;
    }
}