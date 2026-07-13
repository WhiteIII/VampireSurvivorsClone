using System.Collections.Generic;
using _Project.Scripts.Common.Services.Initialize;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.CompositionRoot.Services
{
    public interface IAsyncDependenciesRepository : IAsyncInitializable
    {
        bool IsInitialized { get; }
        IEnumerable<object> Instances { get; }
        UniTask<T> GetInstanceAsync<T>() where T : class;
    }
}