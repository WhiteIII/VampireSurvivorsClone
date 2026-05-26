using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using Fusion;

namespace _Project.Scripts.Gameplay.Network.Services.Repositories
{
    public class PlayerRepository : BaseNetworkObjectsRepository<Player>
    {
        [Networked, Capacity(4)] private NetworkLinkedList<NetworkBehaviour> NetworkLinkedList => default;
        
        public override void Spawned() => 
            Initialize(NetworkLinkedList);
        
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
    }
}