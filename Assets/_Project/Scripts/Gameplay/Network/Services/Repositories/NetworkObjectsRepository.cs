using Fusion;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Repositories
{
    public class NetworkObjectsRepository : BaseNetworkObjectsRepository<NetworkBehaviour>
    {
        private NetworkRunner _networkRunner;

        [Networked] private NetworkLinkedList<NetworkBehaviour> _networkBehaviours { get; } = new();

        [Inject] private void Construct(GeneralNetworkObjectsRepository generalNetworkObjectsRepository)
        {
            _networkRunner = generalNetworkObjectsRepository.CurrentNetworkRunner;
            Initialize(_networkBehaviours);
        }
        
        public void DestroyAllObjects()
        {
            foreach (NetworkBehaviour networkBehaviour in _networkBehaviours)
                _networkRunner.Despawn(networkBehaviour.Object);
            _networkBehaviours.Clear();
        }
    }
}