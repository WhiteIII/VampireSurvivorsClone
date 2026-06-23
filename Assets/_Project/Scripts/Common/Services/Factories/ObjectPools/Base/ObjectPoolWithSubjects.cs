using System;
using R3;

namespace _Project.Scripts.Common.Services.Factories.ObjectPools.Base
{
    public class ObjectPoolWithSubjects<TBase> : IObjectPool<TBase>
    {
        public Observable<TBase> OnGet => _getSubject;
        public Observable<TBase> OnRelease => _releaseSubject;
        
        private readonly Subject<TBase> _getSubject = new();
        private readonly Subject<TBase> _releaseSubject = new();
        private readonly BaseObjectPool<TBase> _pool;

        public ObjectPoolWithSubjects(
            Func<TBase> createMethod,
            Action<TBase> onGetMethod,
            Action<TBase> onReleaseMethod)
        {
            _pool = new BaseObjectPool<TBase>(createMethod, onGetMethod, onReleaseMethod);
        }

        public T Get<T>() where T : TBase
        {
            T item = _pool.Get<T>();
            _getSubject.OnNext(item);
            return item;
        }

        public T Release<T>(T item) where T : TBase
        {
            T releasedItem = _pool.Release(item);
            _releaseSubject.OnNext(releasedItem);
            return releasedItem;
        }
    }

    public class ObjectPoolWithSubjects<TBase, TParameter> : IObjectPool<TBase, TParameter>
    {
        public Observable<TBase> OnGet => _getSubject;
        public Observable<TBase> OnRelease => _releaseSubject;
        
        private readonly Subject<TBase> _getSubject = new();
        private readonly Subject<TBase> _releaseSubject = new();
        private readonly IObjectPool<TBase, TParameter> _pool;

        public ObjectPoolWithSubjects(
            Func<TBase> createMethod,
            Action<TBase, TParameter> onGetMethod,
            Action<TBase, TParameter> onReleaseMethod)
        {
            _pool = new BaseObjectPool<TBase, TParameter>(createMethod, onGetMethod, onReleaseMethod);
        }
        
        public T Get<T>(TParameter parameter) where T : TBase
        {
            T item = _pool.Get<T>(parameter);
            _getSubject.OnNext(item);
            return item;
        }

        public T ReleaseByParameter<T>(TParameter parameter) where T : TBase
        {
            T item = _pool.ReleaseByParameter<T>(parameter);
            _releaseSubject.OnNext(item);
            return item;
        }

        public T Release<T>(T item) where T : TBase
        {
            T releasedItem = _pool.Release(item);
            _releaseSubject.OnNext(releasedItem);
            return item;
        }
    }
}