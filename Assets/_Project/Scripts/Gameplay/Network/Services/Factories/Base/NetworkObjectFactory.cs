using _Project.Scripts.Common.Services.Factories.Base;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Base
{
    public abstract class NetworkObjectFactory<TValue> : BaseNetworkObjectFactory, IFactory<UniTask<TValue>>
        where TValue : NetworkBehaviour
    {
        private readonly INetworkObjectsCreator<TValue> _networkObjectsCreator;
        
        protected NetworkObjectFactory(
            AssetReference prefabAssetReference, 
            INetworkObjectsCreator<TValue> networkObjectsCreator) : base(prefabAssetReference)
        {
            _networkObjectsCreator = networkObjectsCreator;
        }

        public UniTask<TValue> Create() => 
            _networkObjectsCreator.Create<TValue>(PrefabAssetReference);
    }
}