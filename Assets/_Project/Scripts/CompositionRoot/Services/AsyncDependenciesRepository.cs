using System;
using System.Collections.Generic;
using _Project.Scripts.Common.Services.Initialize;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.CompositionRoot.Services
{
    public class AsyncDependenciesRepository : IAsyncInitializable
    {
        private readonly List<IAsyncDependence> _dependencies;
        private readonly List<object> _instances = new();

        private bool InstancesIsDone => _instances.Count == _dependencies.Count;
        
        public AsyncDependenciesRepository(List<IAsyncDependence> dependencies) =>
            _dependencies = dependencies;
        
        public async UniTask InitializeAsync()
        {
            foreach (IAsyncDependence dependence in _dependencies)
            {
                await dependence.InitializeAsync();
                _instances.Add(dependence.ObjectInstance);
            }
        }

        public async UniTask<T> GetInstanceAsync<T>()
        {
            if (InstancesIsDone)
            {
                foreach (object instance in _instances)
                {
                    if (instance is T concreteInstance)
                        return concreteInstance;
                }
            }

            foreach (IAsyncDependence dependence in _dependencies)
            {
                if (dependence is AsyncDependence<T> concreteDependence)
                {
                    await UniTask.WaitWhile(() => dependence.Task.Status != UniTaskStatus.Succeeded);
                    return concreteDependence.Instance;
                }
            }
            throw new Exception("Instance not found!");
        }
    }
}