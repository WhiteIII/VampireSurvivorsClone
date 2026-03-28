using System;
using System.Collections.Generic;
using _Project.Scripts.Common.Services.Repositories.Base;
using _Project.Scripts.Configs.Base;
using _Project.Scripts.Configs.Services.Base;
using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using Fusion;

namespace _Project.Scripts.Gameplay.Network.Services.Repositories
{
    public class PlayerRepository : IRepository<Player>
    {
        private readonly List<Player> _players;
        private readonly int _playerMaxCount;

        public int Count => _players.Count;

        public PlayerRepository(IConfigService configService)
        {
            _playerMaxCount = configService.GetConfig<IGameConfig>().MaxPlayerCountInSession;
            _players = new List<Player>(_playerMaxCount);
        }

        public bool TryGet<T>(out T item) where T : Player
        {
            item = null;
            foreach (Player player in _players)
            {
                if (player is T concreteItem)
                {
                    item = concreteItem;
                    return true;
                }
            }
            return false;
        }

        public T Add<T>(T player) where T : Player
        {
            if (_players.Count == _playerMaxCount)
                throw new Exception("Player count exceeds player max count!");
            
            _players.Add(player);
            return player;
        }

        public void RemoveAndDestroyByPlayerRef(PlayerRef playerRef)
        {
            if (_players.Count == 0)
                return;

            foreach (Player player in _players)
            {
                if (player.PlayerRef == playerRef)
                {
                    
                    _players.Remove(player);
                    return;
                }
            }
        }
        
        public void Remove(Player player) => 
            _players.Remove(player);
    }
}