using System;
using System.Collections.Generic;
using _Project.Scripts.CompositionRoot.Services;
using Cysharp.Threading.Tasks;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public abstract class AdvancedMonoInstallerWithAsyncInjection : AdvancedMonoInstaller
    {
        private IAsyncDependenciesContainer _dependenciesContainer;
        
        private readonly List<Type> _asyncDependenciesTypes = new();

        [Inject] private void Construct(IAsyncDependenciesContainer dependenciesContainer) => 
            _dependenciesContainer = dependenciesContainer;

        private void OnDestroy()
        {
            foreach (Type type in _asyncDependenciesTypes)
                _dependenciesContainer.Unregister(type);
            _asyncDependenciesTypes.Clear();
        }

        protected void BindAsync<TValue, TFactory>() 
            where TFactory : IFactory<UniTask<TValue>> 
            where TValue : class
        {
            _dependenciesContainer.Register(GetAsyncDependenceProvider<TValue, TFactory>());  
            _asyncDependenciesTypes.Add(typeof(TValue));
        }
        
        private AsyncDependenceProvider<TValue, TFactory> GetAsyncDependenceProvider<TValue, TFactory>() 
            where TFactory : IFactory<UniTask<TValue>>
            where TValue : class =>
            new(Container.Instantiate<TFactory>);
    }
}