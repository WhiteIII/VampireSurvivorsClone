using System;
using Cysharp.Threading.Tasks;
using R3;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Services
{
    public class AsyncDependenceProvider<TValue, TFactory> : IAsyncDependenceProvider<TValue>
        where TFactory : IFactory<UniTask<TValue>>
    {
        private readonly TFactory _factory;
        private readonly Subject<Type> _onCreateStarted = new();
        private readonly Subject<Type> _onCreateCompleted = new();
        
        public TValue Value { get; private set; }
        
        public AsyncDependenceProvider(TFactory factory) => 
            _factory = factory;

        public async UniTask CreateAsync()
        {
            _onCreateStarted.OnNext(typeof(TValue));
            Value = await _factory.Create();
            _onCreateCompleted.OnNext(typeof(TValue));
        } 
    }
}