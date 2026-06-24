using System;
using System.Collections.Generic;

namespace _Project.Scripts.Common.Services.Factories.ObjectPools.Base
{
    public class AdvancedObjectPool<TItem> : IObjectPool<TItem>
    {
        private List<TItem> _unreleasedItems = new();
        private List<TItem> _releasedItems = new();
        
        private readonly Func<TItem> _createMethod;
        private readonly Action<TItem> _onGetMethod;
        private readonly Action<TItem> _onReleaseMethod;
        
        public AdvancedObjectPool(
            Func<TItem> createMethod,
            Action<TItem> onGetMethod,
            Action<TItem> onReleaseMethod)
        {
            _createMethod = createMethod;
            _onGetMethod = onGetMethod;
            _onReleaseMethod = onReleaseMethod;
        }
        
        public T Get<T>() 
            where T : TItem
        {
            TItem item;
            if (_releasedItems.Count == 0)
                item = _createMethod();
            else
            {
                item = _releasedItems[0];
                _releasedItems.RemoveAt(0);
            }
            _unreleasedItems.Add(item);
            _onGetMethod?.Invoke(item);
            return (T)item;
        }

        public T Release<T>(T item) 
            where T : TItem
        {
            if (_unreleasedItems.Contains(item) == false)
                return item;
            _unreleasedItems.Remove(item);
            _releasedItems.Add(item);
            _onReleaseMethod?.Invoke(item);
            return item;
        }
        
        public void SetReleasedItems(List<TItem> releasedItems) => 
            _releasedItems = releasedItems;
        
        public void SetUnreleasedItems(List<TItem> unreleasedItems) =>
            _unreleasedItems = unreleasedItems;
    }
}