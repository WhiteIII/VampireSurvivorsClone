using System.Collections.Generic;
using _Project.Scripts.Common.Services.Factories.ObjectPools.Base;
using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using _Project.Scripts.Gameplay.Network.Services.Factories.Implementation;
using _Project.Scripts.ViewModel.Implementation;
using Cysharp.Threading.Tasks;
using Fusion;
using R3;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.ObjectPool
{
    public class EnemyObjectPool : InjectNetworkBehaviour, IAsyncObjectPoolWithObservables<Enemy>, IAfterHostMigration
    {
        public Observable<Enemy> OnGetObservable => _onGetObservable;
        public Observable<Enemy> OnReleaseObservable => _onReleaseObservable;
        
        private readonly Subject<Enemy> _onGetObservable = new();
        private readonly Subject<Enemy> _onReleaseObservable = new();
        private readonly List<Enemy> _enableEnemies = new();
        private readonly List<Enemy> _disableEnemies = new();

        [Networked] private EnemySpawnPositionHelper PositionHelper { get; set; }
        [Networked] private EnemyFactory Factory { get; set; }

        [Inject] private async UniTask Construct(
            IInstantiator instantiator, 
            AsyncDependenciesRepository asyncDependenciesRepository)
        {
            bool stateAuthority = await GetStateAuthorityAsync();

            if (stateAuthority == false)
            {
                EndInitialization();
                return;
            }

            Factory = await asyncDependenciesRepository.GetInstanceAsync<EnemyFactory>();
            PositionHelper = await asyncDependenciesRepository.GetInstanceAsync<EnemySpawnPositionHelper>();
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
            return enemy;
        }

        private void OnGet(Enemy enemy) =>
            _onGetObservable.OnNext(enemy);

        private void OnRelease(Enemy enemy) =>
            _onReleaseObservable.OnNext(enemy);
    }
}