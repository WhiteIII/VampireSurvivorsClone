using Cysharp.Threading.Tasks;

namespace _Project.Scripts.CompositionRoot.Services
{
    public interface IAsyncDependenciesContainer
    {
        UniTask<T> Resolve<T>();
    }
}