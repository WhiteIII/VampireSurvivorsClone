using System;
using System.Collections.Generic;
using System.Linq;
using Fusion;

namespace _Project.Scripts.Gameplay.Network.Services.Repositories
{
    public class NetworkComponentCreationRepository
    {
        private const uint START_ID = 10000;
        
        private readonly Dictionary<uint, Type> _idAndTypes = new();

        private uint _currentId = START_ID;

        public bool TryGetTypeById(uint id, out Type type) => 
            _idAndTypes.TryGetValue(id, out type);

        public uint GetIdByType<T>() where T : NetworkBehaviour => 
            _idAndTypes.First(x => x.Value == typeof(T)).Key;

        public void RegisterTypeAndGetTypeId<T>() where T : NetworkBehaviour
        {
            _currentId++;
            _idAndTypes.Add(_currentId, typeof(T));
        }
    }
}