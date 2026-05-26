using Fusion;

namespace _Project.Scripts.Gameplay.Network.Services.Repositories
{
    public class NetworkObjectsRepository : BaseNetworkObjectsRepository<NetworkBehaviour>
    {
        [Networked, Capacity(32)] private NetworkLinkedList<NetworkBehaviour> NetworkLinkedList => default;
        
        public override void Spawned() => 
            Initialize(NetworkLinkedList);

        public void DestroyAllObjects()
        {
            foreach (NetworkBehaviour networkBehaviour in List)
                Runner.Despawn(networkBehaviour.Object);
            List.Clear();
        }
    }
}