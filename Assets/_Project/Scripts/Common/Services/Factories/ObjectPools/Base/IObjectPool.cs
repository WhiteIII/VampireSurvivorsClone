namespace _Project.Scripts.Common.Services.Factories.ObjectPools.Base
{
    public interface IObjectPool
    {
        
    }
    
    public interface IObjectPool<in TBase> : IObjectPool
    {
        T Get<T>() where T : TBase;
        T Release<T>(T item) where T : TBase;
    }

    public interface IObjectPool<in TBase, in TParameter> : IObjectPool
    {
        T Get<T>(TParameter parameter) where T : TBase;
        T ReleaseByParameter<T>(TParameter parameter) where T : TBase;
        T Release<T>(T item) where T : TBase;
    }
    //TODO Добавить итератор в пул для EnemiesBarsViewModel!
}