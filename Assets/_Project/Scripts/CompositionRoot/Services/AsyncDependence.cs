using _Project.Scripts.Common.Services.Initialize;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.CompositionRoot.Services
{
    public struct AsyncDependence<T> : IAsyncDependence
    {
        private UniTask<T> _task;
        
        public UniTask Task { get; }
        public T Instance { get; private set; }
        public object ObjectInstance { get; private set; }

        public AsyncDependence(UniTask<T> task)
        {
            Task = task;
            _task = task;
            Instance = default;
            ObjectInstance = null;
        }
        public async UniTask InitializeAsync()
        {
            Instance = await _task;
            ObjectInstance = Instance;
        }
    }
}