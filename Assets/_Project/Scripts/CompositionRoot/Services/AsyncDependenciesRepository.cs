using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.CompositionRoot.Services
{
    public class AsyncDependenciesRepository : IAsyncDependenciesRepository
    {
        private readonly List<IAsyncDependence<object>> _dependencies;
        private readonly List<object> _instances = new();

        public bool IsInitialized => InstancesIsDone;
        public IEnumerable<object> Instances => _instances; 
        
        private bool InstancesIsDone => _instances.Count == _dependencies.Count;
        
        public AsyncDependenciesRepository(List<IAsyncDependence<object>> dependencies) =>
            _dependencies = dependencies;
        
        public async UniTask InitializeAsync()
        {
            foreach (IAsyncDependence<object> dependence in _dependencies)
            {
                if (dependence.InstanceCreated == false && dependence.CreatedInProcess == false)
                    await dependence.InitializeAsync();
                _instances.Add(dependence.Instance);
            }
            await OnInitialize();
        }

        public async UniTask<T> GetInstanceAsync<T>()
            where T : class
        {
            if (InstancesIsDone)
            {
                foreach (object instance in _instances)
                {
                    if (instance is T concreteInstance)
                        return concreteInstance;
                }
            }

            foreach (IAsyncDependence<object> dependence in _dependencies)
            {
                if (dependence is AsyncDependence<T> concreteDependence)
                {
                    if (dependence.InstanceCreated == false)
                    {
                        if (dependence.CreatedInProcess)
                            await UniTask.WaitWhile(() => dependence.CreatedInProcess);
                        else
                            await concreteDependence.CreateInstanceAsync();
                    }
                    if (_instances.Contains(dependence.Instance) == false)
                        _instances.Add(dependence.Instance);
                    return concreteDependence.Instance;
                }
            }
            throw new Exception("Instance not found!");
        }
        
        protected void Add(object instance) => 
            _instances.Add(instance);

        protected virtual UniTask OnInitialize() => 
            UniTask.CompletedTask;
    }
}