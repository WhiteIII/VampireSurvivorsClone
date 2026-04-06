using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using Fusion;

namespace _Project.Scripts.Gameplay.Network.Services.Repositories
{
    public class PlayerRepository : BaseNetworkObjectsRepository<Player>
    {
        [Networked] private NetworkLinkedList<Player> _list { get; } = new(); 
        
        public override void Spawned() => 
            Initialize(_list);
    }
}