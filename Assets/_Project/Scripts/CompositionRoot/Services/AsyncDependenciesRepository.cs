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

        public bool IsInitialized => InstancesIsDone;
        public IEnumerable<object> Instances => _instances; 
        
        private bool InstancesIsDone => _instances.Count == _dependencies.Count;
        
        public AsyncDependenciesRepository(List<IAsyncDependence> dependencies) =>
            _dependencies = dependencies;
        
        public async UniTask InitializeAsync()
        {
            foreach (IAsyncDependence dependence in _dependencies)
            {
                if (dependence.InstanceCreated == false && dependence.CreatedInProcess == false)
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
                    if (dependence.InstanceCreated == false)
                    {
                        if (dependence.CreatedInProcess)
                            await UniTask.WaitWhile(() => dependence.CreatedInProcess);
                        else
                            await concreteDependence.CreateInstanceAsync();
                    }
                    if (_instances.Contains(dependence.ObjectInstance) == false)
                        _instances.Add(dependence.ObjectInstance);
                    return concreteDependence.Instance;
                }
            }
            throw new Exception("Instance not found!");
        }
    }
}