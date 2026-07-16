using System;
using Cysharp.Threading.Tasks;
using R3;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Services
{
    public class AsyncDependenciesContainer : IInitializable, IDisposable, IAsyncDependenciesContainer
    {
        public Observable<AsyncDependenciesContainer> ContainerDisposed => _containerDisposed;
        
        private readonly DiContainer _diContainer;
        private readonly AsyncDependenciesGiver _dependenciesGiver = new();
        private readonly Subject<AsyncDependenciesContainer> _containerDisposed = new();
        
        private DiContainer _subContainer;

        public AsyncDependenciesContainer(DiContainer diContainer) => 
            _diContainer = diContainer;

        public void Initialize() => 
            _subContainer = _diContainer.CreateSubContainer();

        public void Dispose() => 
            _containerDisposed.OnNext(this);

        public bool Contains<T>() where T : class => 
            _dependenciesGiver.Contains<T>();

        public void Register<TValue, TFactory>()
            where TFactory : IFactory<UniTask<TValue>>
            where TValue : class
        {
            _subContainer.Bind<TFactory>().AsSingle().WhenInjectedInto<AsyncDependenceProvider<TValue, TFactory>>();
            _dependenciesGiver.AddFactory(_subContainer.Instantiate<AsyncDependenceProvider<TValue, TFactory>>());
        }

        public async UniTask<T> Resolve<T>()
            where T : class
        {
            if (_dependenciesGiver.Contains<T>())
                return await _dependenciesGiver.GetInstanceAsync<T>();
            throw new Exception($"Can't resolve instance of type {typeof(T).FullName}");
        }
    }
}