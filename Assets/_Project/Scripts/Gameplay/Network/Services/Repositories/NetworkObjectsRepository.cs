using Fusion;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Repositories
{
    public class NetworkObjectsRepository : BaseNetworkObjectsRepository<NetworkBehaviour>
    {
        private NetworkRunner _networkRunner;

        [Networked] private NetworkLinkedList<NetworkBehaviour> NetworkBehaviours { get; } = new();

        [Inject] private void Construct(GeneralNetworkObjectsRepository generalNetworkObjectsRepository)
        {
            _networkRunner = generalNetworkObjectsRepository.CurrentNetworkRunner;
            Initialize(NetworkBehaviours);
        }
        
        public void DestroyAllObjects()
        {
            foreach (NetworkBehaviour networkBehaviour in NetworkBehaviours)
                _networkRunner.Despawn(networkBehaviour.Object);
            NetworkBehaviours.Clear();
        }
    }
}