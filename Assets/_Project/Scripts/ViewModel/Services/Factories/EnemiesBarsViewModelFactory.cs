using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network.Services.Factories.ObjectPool;
using _Project.Scripts.ViewModel.Implementation;
using Cysharp.Threading.Tasks;
using Zenject;

namespace _Project.Scripts.ViewModel.Services.Factories
{
    public class EnemiesBarsViewModelFactory : IFactory<UniTask<EnemiesBarsViewModel>>
    {
        private readonly IAsyncDependenciesRepository _repository;
        private readonly IInstantiator _instantiator;
        
        public EnemiesBarsViewModelFactory(IAsyncDependenciesRepository repository, IInstantiator instantiator)
        {
            _repository = repository;
            _instantiator = instantiator;
        }

        public async UniTask<EnemiesBarsViewModel> Create()
        {
            EnemyObjectPool enemyObjectPool = await _repository.GetInstanceAsync<EnemyObjectPool>();
            EnemiesBarsViewModel viewModel = _instantiator.Instantiate<EnemiesBarsViewModel>();
            viewModel.SetObservables(enemyObjectPool.OnGetObservable, enemyObjectPool.OnReleaseObservable);
            return viewModel;
        }
    }
}