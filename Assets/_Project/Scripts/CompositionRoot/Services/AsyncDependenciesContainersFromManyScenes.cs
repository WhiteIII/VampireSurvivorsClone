using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;

namespace _Project.Scripts.CompositionRoot.Services
{
    public class AsyncDependenciesContainersFromManyScenes : IAsyncDependenciesContainer, IDisposable
    {
        private readonly HashSet<(AsyncDependenciesContainer, CompositeDisposable)> _containersPairDisposables = new();

        public void Dispose()
        {
            foreach ((_, CompositeDisposable disposable) in _containersPairDisposables)
                disposable.Dispose();
            _containersPairDisposables.Clear();
        }

        public UniTask<T> Resolve<T>()  
            where T : class
        {
            foreach ((AsyncDependenciesContainer container, _) in _containersPairDisposables)
            {
                if (container.Contains<T>())
                    return container.Resolve<T>();
            }
            throw new Exception($"{nameof(T)} not found!");
        }

        public AsyncDependenciesContainer AddContainer(AsyncDependenciesContainer container)
        {
            (AsyncDependenciesContainer, CompositeDisposable) containerPairDisposable = 
                new(container, new CompositeDisposable());
            container
                .ContainerDisposed
                .Subscribe(RemoveContainer)
                .AddTo(containerPairDisposable.Item2);
            _containersPairDisposables.Add(containerPairDisposable);
            return containerPairDisposable.Item1;
        }

        private void RemoveContainer(AsyncDependenciesContainer disposedContainer)
        {
            foreach ((AsyncDependenciesContainer container, CompositeDisposable disposable) in _containersPairDisposables)
            {
                if (container == disposedContainer)
                {
                    disposable.Dispose();
                    return;
                }
            } 
        }
    }
}