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
        
        protected GameObject CreateEmptyObject(string name) =>
            _instantiator.CreateEmptyGameObject(name);

        protected void AddComponentOnNetworkObject(GameObject gameObject, Type componentType) =>
            _instantiator.InstantiateComponent(componentType, gameObject);
        
        protected T AddComponentOnNetworkObject<T>(GameObject gameObject) where T : Behaviour =>
            _instantiator.InstantiateComponent<T>(gameObject);

        protected override NetworkObject InstantiatePrefab(NetworkRunner networkRunner, NetworkObject prefab)
        {
            if (networkRunner.IsServer)
                return _instantiator.InstantiatePrefab(prefab.gameObject).GetComponent<NetworkObject>();
            return Instantiate(prefab.gameObject).GetComponent<NetworkObject>();
        }
        //TODO Добавить Dictionary в который будут кладся префабы и булевое значение через метод, по которому можно будет определить, нужно инжектить или нет 
    }
}