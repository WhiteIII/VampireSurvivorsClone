using System;
using System.Collections.Generic;

namespace _Project.Scripts.Common.Services.Factories.ObjectPools.Base
{
    public class BaseObjectPool<TItem> : IObjectPool<TItem>
    {
        private readonly List<TItem> _unreleasedItems = new();
        private readonly List<TItem> _releasedItems = new();
        
        private readonly Func<TItem> _createMethod;
        private readonly Action<TItem> _onGetMethod;
        private readonly Action<TItem> _onReleaseMethod;

        public BaseObjectPool(
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
    }

    public class BaseObjectPool<TItem, TParameter> : IObjectPool<TItem, TParameter>
    {
        private readonly IObjectPool<TItem> _baseObjectPool;
        private readonly Action<TItem, TParameter> _onGetMethod;
        private readonly Action<TItem, TParameter> _onReleaseMethod;
        
        private readonly List<(TParameter, TItem)> _gotItems = new();
        
        public BaseObjectPool(
            Func<TItem> createMethod,
            Action<TItem, TParameter> onGetMethod,
            Action<TItem, TParameter> onReleaseMethod)
        {
            _onGetMethod =  onGetMethod;
            _onReleaseMethod = onReleaseMethod;
            _baseObjectPool = new BaseObjectPool<TItem>(createMethod, null, null);
        }
        
        public T Get<T>(TParameter parameter) where T : TItem
        {
            T item = _baseObjectPool.Get<T>();
            _onGetMethod?.Invoke(item, parameter);
            _gotItems.Add((parameter, item));
            return item;
        }

        public T ReleaseByParameter<T>(TParameter parameter) where T : TItem
        {
            T item = _baseObjectPool.Release(GetFromListAndRemove<T>(parameter));
            _onReleaseMethod?.Invoke(item, parameter);
            return item;
        }

        public T Release<T>(T item) where T : TItem
        {
            T releasedItem = _baseObjectPool.Release(item);
            _onReleaseMethod?.Invoke(item, GetFromListAndRemove(item));
            return releasedItem;
        }

        private T GetFromListAndRemove<T>(TParameter parameter) where T : TItem
        {
            foreach ((TParameter, TItem) itemAndParameter in _gotItems)
            {
                if (EqualityComparer<TParameter>.Default.Equals(itemAndParameter.Item1, parameter))
                {
                    _gotItems.Remove(itemAndParameter);
                    return (T)itemAndParameter.Item2;
                }
            }
            throw new Exception("No item found with the given parameter!");
        }

        private TParameter GetFromListAndRemove(TItem item)
        {
            foreach ((TParameter, TItem) itemAndParameter in _gotItems)
            {
                if (EqualityComparer<TItem>.Default.Equals(itemAndParameter.Item2, item))
                {
                    _gotItems.Remove(itemAndParameter);
                    return itemAndParameter.Item1;
                }
            }
            throw new Exception("No item found with the given parameter!");
        }
    }
}