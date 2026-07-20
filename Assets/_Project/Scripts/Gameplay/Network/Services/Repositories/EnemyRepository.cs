using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using Fusion;

namespace _Project.Scripts.Gameplay.Network.Services.Repositories
{
    public class EnemyRepository : BaseNetworkObjectsRepository<Enemy>
    {
        [Networked, Capacity(32)] private NetworkLinkedList<NetworkBehaviour> NetworkLinkedList => default;        
        
        public override void Spawned() =>
            Initialize(NetworkLinkedList);
    }
}