using System;
using Cysharp.Threading.Tasks;
using R3;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Services
{
    public class AsyncDependenceProvider<TValue, TFactory> : IAsyncDependenceProvider<TValue>
        where TFactory : IFactory<UniTask<TValue>>
    {
        private TFactory _factory;
        private readonly Func<TFactory> _factoryFactory;
        
        public TValue Value { get; private set; }
        
        public AsyncDependenceProvider(TFactory factory) => 
            _factory = factory;
        
        public AsyncDependenceProvider(Func<TFactory> factoryFactory) => 
            _factoryFactory = factoryFactory;

        public async UniTask CreateAsync()
        {
            _factory ??= _factoryFactory();
            Value = await _factory.Create();
        } 
    }
}