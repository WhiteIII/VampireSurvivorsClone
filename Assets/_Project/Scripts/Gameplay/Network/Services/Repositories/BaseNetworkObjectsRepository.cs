using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Common.Services.Repositories.Base;
using Fusion;

namespace _Project.Scripts.Gameplay.Network.Services.Repositories
{
    public abstract class BaseNetworkObjectsRepository<TBaseItem> : NetworkBehaviour, IRepository<TBaseItem>
        where TBaseItem : NetworkBehaviour
    {
        private NetworkLinkedList<TBaseItem> _list;

        [Networked] public int Count
        {
            get => _list.Count;
            set { }
        }

        protected void Initialize(NetworkLinkedList<TBaseItem> list) => 
            _list = list;
        
        public bool TryGet<T>(out T item) where T : TBaseItem
        {       
            item = null;
            foreach (TBaseItem networkBehaviour in _list)
            {
                if (networkBehaviour is T concreteItem)
                {
                    item = concreteItem;
                    return true;
                }
            }
            return false;
        }

        public T Add<T>(T obj) where T : TBaseItem
        {
            _list.Add(obj);
            return obj;
        }

        public void Remove(TBaseItem item) => 
            _list.Remove(item);

        public IEnumerator<TBaseItem> GetEnumerator() => 
            _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => 
            GetEnumerator();
    }
}