using R3;

namespace _Project.Scripts.Common.Services.Factories.ObjectPools.Base
{
    public interface IAsyncObjectPoolWithObservables<TBAse> : IAsyncObjectPool<TBAse>
    {
        Observable<TBAse> OnGetObservable { get; }
        Observable<TBAse> OnReleaseObservable { get; }
    }
}