namespace _Project.Scripts.Common.Services.Factories.ObjectPools.Base
{
    public interface IObjectPool<TBase>
    {
        public T Register<T>(T item) where T : TBase;
        public T Unregister<T>() where T : TBase;
        public T Unrelease<T>() where T : TBase;
        public T Release<T>(T item) where T : TBase;
    }
}