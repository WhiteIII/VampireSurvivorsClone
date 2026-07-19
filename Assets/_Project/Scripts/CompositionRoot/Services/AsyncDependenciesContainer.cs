using System;
using Cysharp.Threading.Tasks;
using R3;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Services
{
    public class AsyncDependenciesContainer : IAsyncDependenciesContainer
    {
        private readonly AsyncDependenciesGiver _dependenciesGiver = new();

        public void Register<T>(IAsyncDependenceProvider<T> provider) => 
            _dependenciesGiver.AddFactory(provider);

        public void Unregister<T>() => _dependenciesGiver.Unregister<T>();
        
        public async UniTask<T> Resolve<T>()
            where T : class
        {
            if (_dependenciesGiver.Contains<T>())
                return await _dependenciesGiver.GetInstanceAsync<T>();
            throw new Exception($"Can't resolve instance of type {typeof(T).Name}");
        }
    }
}