using System;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.CompositionRoot.Services
{
    public class AsyncDependence<T> : IAsyncDependence<T>
        where T : class
    {
        private readonly Func<UniTask<T>> _creationMethodAsync;
        
        public bool InstanceCreated { get; private set; }
        public bool CreatedInProcess { get; private set; }
        public T Instance { get; private set; }

        public AsyncDependence(Func<UniTask<T>> creationMethodAsync) => 
            _creationMethodAsync = creationMethodAsync;

        public UniTask InitializeAsync() => 
             CreateInstanceAsync();

        public async UniTask<T> CreateInstanceAsync()
        {
            CreatedInProcess = true;
            Instance = await _creationMethodAsync();
            InstanceCreated = true;
            CreatedInProcess = false;
            return Instance;
        }
    }
}