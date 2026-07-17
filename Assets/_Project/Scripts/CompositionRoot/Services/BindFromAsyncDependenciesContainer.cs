using Cysharp.Threading.Tasks;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Services
{
    public class BindFromAsyncDependenciesContainer<TValue, TFactory> : IFactory<TValue>
        where TFactory : IFactory<UniTask<TValue>>
        where TValue : class
    {
        private readonly AsyncDependenciesContainer _container;

        public BindFromAsyncDependenciesContainer(AsyncDependenciesContainer container) => 
            _container = container;

        public TValue Create()
        {
            _container.Register<TValue, TFactory>();
            return null;
        }
    }
}