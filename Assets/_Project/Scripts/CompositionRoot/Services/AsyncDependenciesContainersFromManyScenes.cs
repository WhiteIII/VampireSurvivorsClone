using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.CompositionRoot.Services
{
    public class AsyncDependenciesContainersFromManyScenes
    {
        private readonly HashSet<AsyncDependenciesContainer> _containers = new();

        public void AddContainer(AsyncDependenciesContainer container) => 
            _containers.Add(container);

        public void RemoveContainer(AsyncDependenciesContainer container) => 
            _containers.Add(container);

        public bool Contains<T>() 
            where T : class
        {
            foreach (var container in _containers)
            {
                if (container.Contains<T>())
                    return true;
            }
            return false;
        }
        
        public UniTask<T> GetInstanceAsync<T>()  
            where T : class
        {
            foreach (var container in _containers)
            {
                if (container.Contains<T>())
                    return container.Resolve<T>();
            }
            throw new Exception($"{nameof(T)} not found!");
        }
    }
}