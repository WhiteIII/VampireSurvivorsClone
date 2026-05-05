using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Common.Services.Repositories.Base;
using _Project.Scripts.Gameplay.Network.Services.HostMigration;
using Fusion;

namespace _Project.Scripts.Gameplay.Network.Services.Repositories
{
    public abstract class BaseNetworkObjectsRepository<TBaseItem> : NetworkBehaviour, IRepository<TBaseItem>, IOnHostMigration
        where TBaseItem : NetworkBehaviour
    {
        protected readonly List<TBaseItem> List = new();
        
        [Networked, Capacity(32)] private NetworkLinkedList<NetworkBehaviour> NetworkObjectIds => default;
        public int Count => List.Count;
        
        public void OnHostMigration(GeneralNetworkObjectsRepository _)
        {
            foreach (NetworkBehaviour networkObjectId in NetworkObjectIds)
                List.Add(networkObjectId.GetComponent<TBaseItem>());
        }

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

        public T Add<T>(T item) where T : TBaseItem
        {
            List.Add(item);
            NetworkObjectIds.Add(item);
            return item;
        }

        public void Remove(TBaseItem item)
        {
            List.Remove(item);
            NetworkObjectIds.Remove(item);
        }

        public IEnumerator<TBaseItem> GetEnumerator() => 
            List.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => 
            GetEnumerator();
    }
}