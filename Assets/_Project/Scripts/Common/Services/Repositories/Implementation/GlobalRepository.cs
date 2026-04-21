using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Common.Services.Repositories.Base;

namespace _Project.Scripts.Common.Services.Repositories.Implementation
{
    public class GlobalRepository : IRepository<object>
    {
        private readonly List<object> _items = new();
        
        public int Count => _items.Count;

        public bool TryGet<T>(out T item)
        {
            item = default;
            foreach (object itemFromList in _items)
            {
                if (itemFromList is T concreteItem)
                {
                    item = concreteItem;
                    return true;
                }
            }
            return false;
        }

        public T Add<T>(T item)
        {
            _items.Add(item);
            return item;
        }

        public void Remove(object item) => 
            _items.Remove(item);

        public IEnumerator<object> GetEnumerator() => 
            _items.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => 
            GetEnumerator();
    }
}