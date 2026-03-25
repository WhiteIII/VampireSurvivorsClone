using _Project.Scripts.Common.Services.Factories.Base;
using Fusion;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories
{
    public abstract class BaseNetworkObjectFactory<TValue> :  IFactory
        where TValue : NetworkBehaviour
    {
        protected readonly AssetReference PrefabAssetReference;

        protected BaseNetworkObjectFactory(AssetReference prefabAssetReference) =>
            PrefabAssetReference = prefabAssetReference;
    }

    public abstract class NetworkObjectFactory<TValue> : BaseNetworkObjectFactory<TValue>, IFactory<TValue>
        where TValue : NetworkBehaviour
    {
        private readonly INetworkObjectsCreator<TValue> _networkObjectsCreator;
        
        protected NetworkObjectFactory(
            AssetReference prefabAssetReference, 
            INetworkObjectsCreator<TValue> networkObjectsCreator) : base(prefabAssetReference)
        {
            _networkObjectsCreator = networkObjectsCreator;
        }

        public TValue Create() => 
            _networkObjectsCreator.Create<TValue>(PrefabAssetReference);
    }
}