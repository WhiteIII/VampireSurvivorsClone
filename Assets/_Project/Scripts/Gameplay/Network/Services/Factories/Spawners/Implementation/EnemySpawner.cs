using System.Collections.Generic;
using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Configs.Base;
using _Project.Scripts.Configs.Services.Base;
using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using _Project.Scripts.Gameplay.Network.Services.Factories.ObjectPool;
using _Project.Scripts.Gameplay.Network.Services.Factories.Spawners.Base;
using _Project.Scripts.Gameplay.Network.Services.GameCycle;
using Cysharp.Threading.Tasks;
using Fusion;
using R3;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Spawners.Implementation
{
    public class EnemySpawner : InjectNetworkBehaviour, ISpawner, IUpdatable, IAfterHostMigration
    {
        private const int MAX_SPAWNED_ENEMIES = 16;
        
        private readonly Dictionary<Observable<Character>, CompositeDisposable> _deadObservers = new(MAX_SPAWNED_ENEMIES);

        private bool SpawnIsNotValid => HasStateAuthority == false
                                    || IsInitializeEnd == false
                                    || IsActive == false
                                    || SpawnedEnemies.Count == MAX_SPAWNED_ENEMIES;
        
        [Networked] private TickTimer Timer { get; set; }
        [Networked] private EnemyObjectPool Pool { get; set; }
        [Networked, UnityNonSerialized] private NetworkBool IsActive { get; set; } = false;
        [Networked, UnityNonSerialized] private float SpawnCooldown { get; set; }
        [Networked, Capacity(MAX_SPAWNED_ENEMIES)] private NetworkLinkedList<Enemy> SpawnedEnemies => default;

        [Inject] private async UniTask Construct(
            IConfigService configService, 
            AsyncDependenciesRepository asyncDependenciesRepository)
        {
            bool hasStateAuthority = await GetStateAuthorityAsync();
            if (hasStateAuthority == false)
            {
                EndInitialization();
                return;
            }

            SpawnCooldown = configService.GetConfig<IGameConfig>().EnemySpawnCooldown;
            Pool = await asyncDependenciesRepository.GetInstanceAsync<EnemyObjectPool>();
            EndInitialization();
        }

        protected override void OnSpawnMethod() => 
            Timer = TickTimer.CreateFromSeconds(Runner, SpawnCooldown);

        public void AfterHostMigration()
        {
            foreach (Enemy enemy in SpawnedEnemies)
            {
                CompositeDisposable disposable = new();
                enemy
                    .OnDead
                    .Subscribe(x => RemoveEnemy((Enemy)x))
                    .AddTo(disposable);
                _deadObservers.Add(enemy.OnDead, disposable);
            }
        }

        private void OnDestroy()
        {
            foreach (CompositeDisposable disposable in _deadObservers.Values)
                disposable.Dispose();
            _deadObservers.Clear();
        }
        
        public void GameLoopUpdate()
        {
            if (SpawnIsNotValid)
                return;
            
            if (Timer.Expired(Runner))
            {
                Timer = TickTimer.CreateFromSeconds(Runner, SpawnCooldown);   
                Spawn().Forget();
            }
        }

        public void Enable()
        {
            if (HasStateAuthority == false)
                return;
            
            IsActive = true;
        }

        public void Disable()
        {
            if (HasStateAuthority == false)
                return;
            
            IsActive = false;
        }

        private async UniTask Spawn()
        {
            Enemy spawnedEnemy = await Pool.GetAsync<Enemy>();
            AddEnemy(spawnedEnemy);
        }

        private void AddEnemy(Enemy enemy)
        {
            SpawnedEnemies.Add(enemy);
            Observable<Character> deadObserver = enemy.OnDead;
            CompositeDisposable disposable = new();
            deadObserver
                .Subscribe(x => RemoveEnemy((Enemy)x))
                .AddTo(disposable);
            _deadObservers.Add(deadObserver, disposable);
        }

        private void RemoveEnemy(Enemy enemy)
        {
            SpawnedEnemies.Remove(enemy);
            if (_deadObservers.ContainsKey(enemy.OnDead))
            {
                _deadObservers[enemy.OnDead].Dispose();
                _deadObservers.Remove(enemy.OnDead);
            }
        }
    }
}