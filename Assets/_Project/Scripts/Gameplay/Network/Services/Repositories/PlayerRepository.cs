using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using _Project.Scripts.Gameplay.Network.Services.Factories;
using Fusion;

namespace _Project.Scripts.Gameplay.Network.Services.Repositories
{
    public class PlayerRepository : BaseNetworkObjectsRepository<Player>
    {
        [Networked] private NetworkLinkedList<Player> List { get; } = new(); 
        
        public override void Spawned() => 
            Initialize(List);
    }
}