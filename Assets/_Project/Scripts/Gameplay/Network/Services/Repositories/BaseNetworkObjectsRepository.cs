using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Common.Services.Repositories.Base;
using Fusion;

namespace _Project.Scripts.Gameplay.Network.Services.Repositories
{
    public abstract class BaseNetworkObjectsRepository<TBaseItem> : NetworkBehaviour, IRepository<TBaseItem>
        where TBaseItem : NetworkBehaviour
    {
        protected readonly List<TBaseItem> List = new();
        
        public int Count => List.Count;

        public bool TryGet<T>(out T item) where T : TBaseItem
        {       
            item = null;
            foreach (TBaseItem networkBehaviour in List)
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
            List.Add(obj);
            return obj;
        }

        public void Remove(TBaseItem item) => 
            List.Remove(item);

        public IEnumerator<TBaseItem> GetEnumerator() => 
            List.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => 
            GetEnumerator();
    }
}