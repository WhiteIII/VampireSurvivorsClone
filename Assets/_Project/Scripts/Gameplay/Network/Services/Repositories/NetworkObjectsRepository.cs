using Fusion;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Repositories
{
    public class NetworkObjectsRepository : BaseNetworkObjectsRepository<NetworkBehaviour>
    {
        private NetworkRunner _networkRunner;
        
        [Inject] private void Construct(GeneralNetworkObjectsRepository generalNetworkObjectsRepository) => 
            _networkRunner = generalNetworkObjectsRepository.CurrentNetworkRunner;

        public void DestroyAllObjects()
        {
            foreach (NetworkBehaviour networkBehaviour in List)
                _networkRunner.Despawn(networkBehaviour.Object);
            List.Clear();
        }
    }
}