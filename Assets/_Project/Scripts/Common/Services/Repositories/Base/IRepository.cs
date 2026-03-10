namespace _Project.Scripts.Common.Repositories.Base
{
    public interface IRepository<in TBaseItem>
    {
        T Add<T>(T obj) where T : TBaseItem;
    }
}
