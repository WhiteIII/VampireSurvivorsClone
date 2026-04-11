using System;
using Fusion;
using UnityEngine;
using Zenject;
using Behaviour = Fusion.Behaviour;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.NetworkObjectProvider
{
    public class NetworkObjectProviderDefaultWithInject : NetworkObjectProviderDefault
    {
        private IInstantiator _instantiator;

        [Inject] private void Construct(IInstantiator instantiator) =>
            _instantiator = instantiator;

        protected GameObject CreateEmptyObject() =>
            _instantiator.CreateEmptyGameObject("Clone");

        protected void AddComponentOnNetworkObject(GameObject gameObject, Type componentType) =>
            _instantiator.InstantiateComponent(componentType, gameObject);
        protected T AddComponentOnNetworkObject<T>(GameObject gameObject) where T : Behaviour =>
            _instantiator.InstantiateComponent<T>(gameObject);
        
        protected override NetworkObject InstantiatePrefab(NetworkRunner _, NetworkObject prefab) => 
            _instantiator.InstantiatePrefab(prefab.gameObject).GetComponent<NetworkObject>();
    }
}