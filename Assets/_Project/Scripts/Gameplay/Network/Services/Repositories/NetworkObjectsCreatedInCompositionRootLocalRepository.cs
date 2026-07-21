using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Common.Services.Repositories.Base;
using Fusion;
using R3;

namespace _Project.Scripts.Gameplay.Network.Services.Repositories
{
    public class NetworkObjectsCreatedInCompositionRootLocalRepository : IRepository<NetworkBehaviour>
    {
        public Observable<NetworkBehaviour> OnAdd => _onAdd;
        
        private readonly List<NetworkBehaviour> _list = new();
        private readonly Subject<NetworkBehaviour> _onAdd = new();
        
        public int Count => _list.Count;

        public bool TryGet<T>(out T item) where T : NetworkBehaviour
        {
            item = null;
            foreach (NetworkBehaviour networkBehaviour in _list)
            {
                if (networkBehaviour is T concreteNetworkBehaviour)
                {
                    item = concreteNetworkBehaviour;
                    return true;
                }
            }
            return false;
        }

        public T Add<T>(T item) where T : NetworkBehaviour
        {
            _list.Add(item);
            _onAdd.OnNext(item);
            return item;
        }

        public void Remove(NetworkBehaviour item) => 
            _list.Remove(item);

        public IEnumerator<NetworkBehaviour> GetEnumerator() => 
            _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
}