using System;
using System.Collections.Generic;
using _Project.Scripts.Common.Repositories.Base;
using _Project.Scripts.Configs.Base;
using _Project.Scripts.Configs.Services.Base;
using _Project.Scripts.Gameplay.Network.Services.BaseComponent;

namespace _Project.Scripts.Gameplay.Network.Services.Repositories
{
    public class PlayerRepository : IRepository<Player>
    {
        private readonly List<Player> _players;
        private readonly int _playerMaxCount;

        public PlayerRepository(IConfigService configService)
        {
            _playerMaxCount = configService.GetConfig<IGameConfig>().MaxPlayerCountInSession;
            _players = new List<Player>(_playerMaxCount);
        } 
        
        public T Add<T>(T player) where T : Player
        {
            if (_players.Count == _playerMaxCount)
                throw new Exception("Player count exceeds player max count!");
            
            _players.Add(player);
            return player;
        }
        
        public void Remove(Player player) => 
            _players.Remove(player);
    }
}