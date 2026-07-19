using System;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.CompositionRoot.Services
{
    public interface IAsyncDependenciesContainer
    {
        void Register<T>(IAsyncDependenceProvider<T> provider);
        void Unregister(Type type);
        UniTask<T> Resolve<T>() where T : class;
    }
}