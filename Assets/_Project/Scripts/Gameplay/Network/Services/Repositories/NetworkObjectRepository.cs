using System.Collections.Generic;
using _Project.Scripts.Common.Repositories.Base;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Network.Services.Repositories
{
    public class NetworkObjectRepository : IRepository<NetworkBehaviour>
    {
        private readonly GeneralNetworkObjectsRepository _generalNetworkObjectsRepository;
        private readonly List<NetworkBehaviour> _networkBehaviours = new();

        public NetworkObjectRepository(GeneralNetworkObjectsRepository generalNetworkObjectsRepository) => 
            _generalNetworkObjectsRepository = generalNetworkObjectsRepository;

        public T Add<T>(T networkBehaviour) where T : NetworkBehaviour
        {
            _networkBehaviours.Add(networkBehaviour);
            return networkBehaviour;
        }

        public void DestroyAllObjects()
        {
            foreach (NetworkBehaviour networkBehaviour in _networkBehaviours)
                _generalNetworkObjectsRepository.CurrentNetworkRunner.Despawn(networkBehaviour.Object);
            _networkBehaviours.Clear();
        }
    }
}