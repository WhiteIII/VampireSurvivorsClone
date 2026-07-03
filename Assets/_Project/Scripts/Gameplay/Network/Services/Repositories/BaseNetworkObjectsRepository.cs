using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Common.Services.Repositories.Base;
using Fusion;

namespace _Project.Scripts.Gameplay.Network.Services.Repositories
{
    public abstract class BaseNetworkObjectsRepository<TBaseItem> : NetworkBehaviour, IRepository<TBaseItem>,
        IAfterHostMigration
        where TBaseItem : NetworkBehaviour
    {
        protected readonly List<TBaseItem> List = new();

        private NetworkLinkedList<NetworkBehaviour> _networkObjects;

        public int Count => List.Count;

        protected void Initialize(NetworkLinkedList<NetworkBehaviour> networkObjects)
        {
            _networkObjects = networkObjects;
            if (HasStateAuthority)
                AddAllNetworkObjectsToList();
        }

        public void AfterHostMigration() =>
            AddAllNetworkObjectsToList();

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
            _networkObjects.Add(item);
            return item;
        }

        public void Remove(TBaseItem item)
        {
            List.Remove(item);
            _networkObjects.Remove(item);
        }

        public IEnumerator<TBaseItem> GetEnumerator() =>
            List.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();

        private void AddAllNetworkObjectsToList()
        {
            foreach (NetworkBehaviour networkObjectId in _networkObjects)
                List.Add(networkObjectId.GetComponent<TBaseItem>());
        }
    }
}