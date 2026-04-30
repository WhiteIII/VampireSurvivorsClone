using Fusion;

namespace _Project.Scripts.Gameplay.Network.Services.Repositories
{
    public class NetworkObjectsRepository : BaseNetworkObjectsRepository<NetworkBehaviour>
    {
        public void DestroyAllObjects()
        {
            foreach (NetworkBehaviour networkBehaviour in List)
                Runner.Despawn(networkBehaviour.Object);
            List.Clear();
        }
    }
}