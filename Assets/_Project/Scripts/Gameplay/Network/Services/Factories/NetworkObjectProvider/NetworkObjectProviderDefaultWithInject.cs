using System;
using Fusion;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.NetworkObjectProvider
{
    public class NetworkObjectProviderDefaultWithInject : NetworkObjectProviderDefault
    {
        private IInstantiator _instantiator;

        [Inject] private void Construct(IInstantiator instantiator) =>
            _instantiator = instantiator;

        protected NetworkObject CreateEmptyObject() =>
            _instantiator.InstantiateComponentOnNewGameObject<NetworkObject>();

        protected void AddComponentOnNetworkObject(NetworkObject networkObject, Type componentType) =>
            _instantiator.InstantiateComponent(componentType, networkObject.gameObject);
        
        protected override NetworkObject InstantiatePrefab(NetworkRunner _, NetworkObject prefab) => 
            _instantiator.InstantiatePrefab(prefab.gameObject).GetComponent<NetworkObject>();
    }
}