using _Project.Scripts.Common.Services.Factories.Base;
using Fusion;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories
{
    public abstract class NetworkObjectFactory<TValue> : BaseNetworkObjectFactory, IFactory<TValue>
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