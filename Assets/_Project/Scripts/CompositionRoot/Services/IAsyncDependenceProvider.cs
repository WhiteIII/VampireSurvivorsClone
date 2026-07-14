using Cysharp.Threading.Tasks;

namespace _Project.Scripts.CompositionRoot.Services
{
    public interface IAsyncDependenceProvider<out T>
    {
        T Value { get; }
        UniTask CreateAsync();
    }
}