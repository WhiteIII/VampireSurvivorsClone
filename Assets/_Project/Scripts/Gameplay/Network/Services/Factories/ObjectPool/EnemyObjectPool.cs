using System;
using _Project.Scripts.Common.Services.Factories.ObjectPools.Base;
using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using Fusion;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.ObjectPool
{
    public class EnemyObjectPool : NetworkBehaviour, IObjectPool<Character>
    {
        private readonly BaseObjectPool<Character> _pool;

        private void Awake()
        {
            
        }
        
        public T Get<T>() where T : Character
        {
            throw new NotImplementedException();
        }

        public T Release<T>(T item) where T : Character
        {
            throw new NotImplementedException();
        }
    }
}