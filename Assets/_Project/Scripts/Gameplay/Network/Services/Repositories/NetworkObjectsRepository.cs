using Fusion;

namespace _Project.Scripts.Gameplay.Network.Services.Repositories
{
    public class NetworkObjectsRepository : BaseNetworkObjectsRepository<NetworkBehaviour>
    {
        [Networked] private NetworkLinkedList<NetworkBehaviour> NetworkBehaviours { get; } = new();

        public override void Spawned() => 
            Initialize(NetworkBehaviours);

        public void DestroyAllObjects()
        {
            foreach (NetworkBehaviour networkBehaviour in NetworkBehaviours)
                Runner.Despawn(networkBehaviour.Object);
            NetworkBehaviours.Clear();
        }
    }
}