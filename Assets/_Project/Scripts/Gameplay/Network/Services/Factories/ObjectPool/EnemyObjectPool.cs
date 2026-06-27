using System.Collections.Generic;
using _Project.Scripts.Common.Services.Factories.ObjectPools.Base;
using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using _Project.Scripts.Gameplay.Network.Services.Factories.Implementation;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using _Project.Scripts.Gameplay.Services;
using _Project.Scripts.ViewModel.Implementation;
using Cysharp.Threading.Tasks;
using Fusion;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.ObjectPool
{
    public class EnemyObjectPool : InjectNetworkBehaviour, IAsyncObjectPool<Enemy>, IAfterHostMigration
    {
        private EnemiesBarsViewModel _viewModel;

        private readonly List<Enemy> _enableEnemies = new();
        private readonly List<Enemy> _disableEnemies = new();
        
        [Networked] private EnemySpawnPositionHelper PositionHelper { get; set; }
        [Networked] private IdGenerator IdGenerator { get; set; }
        [Networked] private EnemyFactory Factory { get; set; }
        [Networked] private EnemyRepository Repository { get; set; }
        
        [Inject] private async UniTask Construct(
            IInstantiator instantiator, 
            EnemiesBarsViewModel viewModel,
            AsyncDependenciesRepository asyncDependenciesRepository)
        {
            _viewModel = viewModel;
            bool stateAuthority = await GetStateAuthorityAsync();

            if (stateAuthority)
            {
                EndInitialization();
                return;
            }

            Factory = await asyncDependenciesRepository.GetInstanceAsync<EnemyFactory>();
            IdGenerator = await asyncDependenciesRepository.GetInstanceAsync<IdGenerator>();
            PositionHelper = await asyncDependenciesRepository.GetInstanceAsync<EnemySpawnPositionHelper>();
            Repository = await asyncDependenciesRepository.GetInstanceAsync<EnemyRepository>();
            EndInitialization();
        }

        public void AfterHostMigration()
        {
            List<NetworkObject> networkObjects = Runner.GetAllNetworkObjects();

            foreach (NetworkObject networkObject in networkObjects)
            {
                if (networkObject.TryGetComponent(out Enemy enemy) == false)
                    continue;
                if (enemy.IsActive)
                    _enableEnemies.Add(enemy);
                else
                    _disableEnemies.Add(enemy);
            }
        }

        public async UniTask<T> GetAsync<T>() 
            where T : Enemy
        {
            Enemy item;
            if (_disableEnemies.Count == 0)
                item = await Create();
            else
            {
                item = _disableEnemies[0];
                _disableEnemies.RemoveAt(0);
            }
            _enableEnemies.Add(item);
            OnGet(item);
            return (T)item;            
        }

        public T Release<T>(T item) where T : Enemy
        {
            if (_enableEnemies.Contains(item) == false)
                return item;
            _enableEnemies.Remove(item);
            _disableEnemies.Add(item);
            OnRelease(item);
            return item;            
        }

        private async UniTask<Enemy> Create()
        {
            Enemy enemy = await Factory.Create(PositionHelper.GetSpawnPosition());
            enemy.SetId(IdGenerator.GetId());
            return enemy;
        }
        
        private void OnGet(Enemy enemy) =>
            _viewModel.Add(new EnemyBarViewModelData
            {
                OnPositionChanged = enemy.Position,
                OnHealthChanged = enemy.OnHealthChanged,
                OnMaxHealthChanged = enemy.OnMaxHealthChanged,
                EnemyId = enemy.EnemyId
            });

        private void OnRelease(Enemy enemy) => 
            _viewModel.Remove(enemy.EnemyId);
    }
}