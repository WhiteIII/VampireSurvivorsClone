using System;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.CompositionRoot.Services
{
    public class AsyncDependence<T> : IAsyncDependence
    {
        private readonly Func<UniTask<T>> _creationMethodAsync;
        
        public bool InstanceCreated { get; private set; }
        public bool CreatedInProcess { get; private set; }
        public T Instance { get; private set; }
        public object ObjectInstance { get; private set; }

        public AsyncDependence(Func<UniTask<T>> creationMethodAsync)
        {
            _creationMethodAsync = creationMethodAsync;
            InstanceCreated = false;
            CreatedInProcess = false;
            Instance = default;
            ObjectInstance = null;
        }

        public UniTask InitializeAsync() => 
             CreateInstanceAsync();

        public async UniTask<T> CreateInstanceAsync()
        {
            CreatedInProcess = true;
            Instance = await _creationMethodAsync();
            ObjectInstance = Instance;
            InstanceCreated = true;
            CreatedInProcess = false;
            return Instance;
        }
    }
}