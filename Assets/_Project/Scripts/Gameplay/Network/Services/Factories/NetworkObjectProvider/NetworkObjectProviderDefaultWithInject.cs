using System;
using System.Collections.Generic;
using Fusion;
using UnityEngine;
using Zenject;
using Behaviour = Fusion.Behaviour;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.NetworkObjectProvider
{
    public class NetworkObjectProviderDefaultWithInject : NetworkObjectProviderDefault
    {
        private IInstantiator _instantiator;
        private DiContainer _container;
        
        private readonly Dictionary<NetworkObject, bool> _objectAndInjectionFlagPairs = new();
        private readonly Dictionary<Type, bool> _typeAndInjectionFlagPairs = new();

        [Inject] private void Construct(IInstantiator instantiator, DiContainer container)
        {
            _instantiator = instantiator;
            _container = container;
        } 
        
        public void AddObjectAndInjectionFlagPair(NetworkObject networkObject, bool isWithInjection) => 
            _objectAndInjectionFlagPairs.Add(networkObject, isWithInjection);
        
        public void AddTypeAndInjectionFlagPair(Type type, bool isWithInjection) => 
            _typeAndInjectionFlagPairs.Add(type, isWithInjection);
        
        protected GameObject CreateEmptyObject(string name) =>
            _instantiator.CreateEmptyGameObject(name);

        protected void AddComponentOnNetworkObject(GameObject gameObject, Type componentType)
        {
            if (_typeAndInjectionFlagPairs.TryGetValue(componentType, out var isWithInjection))
            {
                if (isWithInjection)
                {
                    _typeAndInjectionFlagPairs.Remove(componentType);
                    _instantiator.InstantiateComponent(componentType, gameObject);
                }
                else 
                    gameObject.AddComponent(componentType);
                return;
            }
            _instantiator.InstantiateComponent(componentType, gameObject);
        }

        protected T AddComponentOnNetworkObject<T>(GameObject gameObject) where T : Behaviour
        {
            if (_typeAndInjectionFlagPairs.TryGetValue(typeof(T), out var isWithInjection))
            {
                if (isWithInjection)
                {
                    _typeAndInjectionFlagPairs.Remove(typeof(T));
                    return _instantiator.InstantiateComponent<T>(gameObject);
                }
                return gameObject.AddComponent<T>();
            }
            return _instantiator.InstantiateComponent<T>(gameObject);
        }

        protected override NetworkObject InstantiatePrefab(NetworkRunner networkRunner, NetworkObject prefab)
        {
            if (_objectAndInjectionFlagPairs.TryGetValue(prefab, out var withInjection))
            {
                if (withInjection)
                {
                    _objectAndInjectionFlagPairs.Remove(prefab);
                    return _instantiator.InstantiatePrefab(prefab.gameObject).GetComponent<NetworkObject>();
                }
                return Instantiate(prefab.gameObject).GetComponent<NetworkObject>();
            }
            return _instantiator.InstantiatePrefab(prefab.gameObject).GetComponent<NetworkObject>();
        }
    }
}