using _Project.Scripts.Common.Services.Factories.ObjectPools.Base;
using _Project.Scripts.View.Base;
using _Project.Scripts.ViewModel.Implementation;
using Cysharp.Threading.Tasks;
using R3;
using Zenject;

namespace _Project.Scripts.View.Implementation
{
    public class EnemiesBarsWindow : Window<EnemiesBarsViewModel>
    {
        private BaseObjectPool<EnemyBar, EnemyBarViewModel> _enemiesBarPool;
        private IFactory<EnemyBar> _factory;
        
        [Inject] private void Construct(IFactory<EnemyBar> factory) => 
            _factory = factory;  
        
        protected override void OnAwakeMethodIfViewModelIsNotNull()
        {
            _enemiesBarPool = new BaseObjectPool<EnemyBar, EnemyBarViewModel>(
                _factory.Create, OnGetEnemyBar, OnReleaseEnemyBar);
            ViewModel
                .OnEnemyBarAdded
                .Subscribe(x => _enemiesBarPool.Get<EnemyBar>(x))
                .AddTo(this);
            ViewModel
                .OnEnemyBarRemoved
                .Subscribe(x => _enemiesBarPool.ReleaseByParameter<EnemyBar>(x))
                .AddTo(this);
        }

        private void OnGetEnemyBar(EnemyBar enemyBar, EnemyBarViewModel enemyBarViewModel)
        {
            enemyBar.SetViewModel(enemyBarViewModel);
            enemyBar.OpenAsync().Forget();
        }

        private void OnReleaseEnemyBar(EnemyBar enemyBar, EnemyBarViewModel _)
        {
            enemyBar.Release();
            enemyBar.CloseAsync().Forget();
        }
    }
}