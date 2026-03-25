using System.Collections.Generic;
using _Project.Scripts.Common.Repositories.Base;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Network.Services.Repositories
{
    public class NetworkObjectsRepository : IRepository<NetworkBehaviour>
    {
        private readonly GeneralNetworkObjectsRepository _generalNetworkObjectsRepository;
        private readonly List<NetworkBehaviour> _networkBehaviours = new();

        public NetworkObjectsRepository(GeneralNetworkObjectsRepository generalNetworkObjectsRepository) => 
            _generalNetworkObjectsRepository = generalNetworkObjectsRepository;

        public bool TryGet<T>(out T item) where T : NetworkBehaviour
        {
            item = null;
            foreach (NetworkBehaviour networkBehaviour in _networkBehaviours)
            {
                if (networkBehaviour is T concreteItem)
                {
                    item = concreteItem;
                    return true;
                }
            }
            return false;
        }

        public T Add<T>(T networkBehaviour) where T : NetworkBehaviour
        {
            _networkBehaviours.Add(networkBehaviour);
            return networkBehaviour;
        }

        public void Remove(NetworkBehaviour item) => 
            _networkBehaviours.Remove(item);

        public void DestroyAllObjects()
        {
            foreach (NetworkBehaviour networkBehaviour in _networkBehaviours)
                _generalNetworkObjectsRepository.CurrentNetworkRunner.Despawn(networkBehaviour.Object);
            _networkBehaviours.Clear();
        }
    }
}