using System;
using Cysharp.Threading.Tasks;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Services
{
    public class LayerAboveAsyncFactory<TType, TFactory> : IFactory<AsyncDependence<TType>>
        where TFactory : IFactory<UniTask<TType>>
        where TType : class
    {
        private readonly TFactory _factory;
        
        public LayerAboveAsyncFactory(IInstantiator instantiator) => 
            _factory = instantiator.Instantiate<TFactory>();
        
        public AsyncDependence<TType> Create() => 
            new(_factory.Create);
    }

    public class EmptyZenjectDependence<T> : IEmptyZenjectDependence<T>
    {
        public Type DerivativeOfType => typeof(T);
    }

    public interface IEmptyZenjectDependence<out T>
    {
        Type DerivativeOfType { get; }
    }

    public class EmptyZenjectDependenceRepository
    {
        
    }
}