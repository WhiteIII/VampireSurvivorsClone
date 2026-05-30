using System.Collections.Generic;
using _Project.Scripts.Common.Services.Factories.Base;
using NUnit.Framework;
using Zenject;

namespace _Project.Scripts.Common.Services.Factories.ObjectPools.Base
{
    public class BaseObjectPool<TBaseItem, TCreator> : IObjectPool<TBaseItem>
        where TCreator : IFactory<TBaseItem>
    {
        private readonly List<TBaseItem> _unreleasedItems = new();
        private readonly List<TBaseItem> _releasedItems = new();
        
        public T Register<T>(T item) where T : TBaseItem
        {
            throw new System.NotImplementedException();
        }

        public T Unregister<T>() where T : TBaseItem
        {
            throw new System.NotImplementedException();
        }

        public T Unrelease<T>() where T : TBaseItem
        {
            throw new System.NotImplementedException();
        }

        public T Release<T>(T item) where T : TBaseItem
        {
            throw new System.NotImplementedException();
        }
    }
}