using Cysharp.Threading.Tasks;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Services
{
    public class BindFromAsyncDependenciesContainer<TValue, TFactory> : IFactory<TValue>
        where TFactory : IFactory<UniTask<TValue>>
        where TValue : class
    {
        private readonly IAsyncDependenciesContainer _asyncDependenciesContainer;
        private readonly DiContainer _diContainer;
        
        public BindFromAsyncDependenciesContainer(
            IAsyncDependenciesContainer asyncDependenciesContainer, 
            DiContainer diContainer)
        {
            _asyncDependenciesContainer = asyncDependenciesContainer;
            _diContainer = diContainer;
        }

        public TValue Create()
        {
            DiContainer subContainer = _diContainer.CreateSubContainer();
            subContainer.Bind<TFactory>().AsSingle().WhenInjectedInto<AsyncDependenceProvider<TValue, TFactory>>();
            _asyncDependenciesContainer.Register(subContainer.Instantiate<AsyncDependenceProvider<TValue, TFactory>>());
            return null;
        }
    }
}