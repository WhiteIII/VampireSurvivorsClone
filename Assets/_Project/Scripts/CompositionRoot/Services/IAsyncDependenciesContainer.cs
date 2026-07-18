using Cysharp.Threading.Tasks;

namespace _Project.Scripts.CompositionRoot.Services
{
    public interface IAsyncDependenciesContainer
    {
        void Register<TValue>(IAsyncDependenceProvider<TValue> provider);
        UniTask<T> Resolve<T>() where T : class;
    }
}