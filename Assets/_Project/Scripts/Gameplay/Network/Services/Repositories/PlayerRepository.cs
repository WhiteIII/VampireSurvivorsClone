using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using _Project.Scripts.Gameplay.Network.Services.Factories;
using Fusion;

namespace _Project.Scripts.Gameplay.Network.Services.Repositories
{
    public class PlayerRepository : BaseNetworkObjectsRepository<Player>
    {
        [Networked] private NetworkLinkedList<Player> List { get; } = new();

        public bool TryGetByPlayerRef(out Player player, PlayerRef playerRef)
        {
            player = null;
            foreach (Player playerFromList in List)
            {
                if (playerFromList.PlayerRef == playerRef)
                {
                    player = playerFromList;
                    return true;
                }
            }
            
            return false;
        }
        
        public override void Spawned() => 
            Initialize(List);
    }
}