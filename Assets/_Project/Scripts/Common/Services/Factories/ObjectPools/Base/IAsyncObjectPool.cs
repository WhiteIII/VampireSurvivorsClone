using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Common.Services.Factories.ObjectPools.Base
{
    public interface IAsyncObjectPool<in TBase> : IObjectPool 
    {
        UniTask<T> GetAsync<T>() where T : TBase;
        T Release<T>(T item) where T : TBase;
    }
}