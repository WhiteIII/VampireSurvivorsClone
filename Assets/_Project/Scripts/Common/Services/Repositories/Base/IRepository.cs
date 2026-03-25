namespace _Project.Scripts.Common.Repositories.Base
{
    public interface IRepository<in TBaseItem>
    {
        bool TryGet<T>(out T item) where T : TBaseItem;
        T Add<T>(T obj) where T : TBaseItem;
        void Remove(TBaseItem item);
    }
}
