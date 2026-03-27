namespace _Project.Scripts.Common.Services.Repositories.Base
{
    public interface IRepository<in TBaseItem>
    {
        int Count { get; }
        bool TryGet<T>(out T item) where T : TBaseItem;
        T Add<T>(T obj) where T : TBaseItem;
        void Remove(TBaseItem item);
    }
}
