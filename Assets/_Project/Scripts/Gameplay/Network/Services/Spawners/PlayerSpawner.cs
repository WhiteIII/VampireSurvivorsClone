using System;
using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using _Project.Scripts.Gameplay.Network.Services.Factories.Implementation;
using _Project.Scripts.Gameplay.Network.Services.HostMigration;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Cysharp.Threading.Tasks;
using Fusion;
using R3;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Spawners
{
    public class PlayerSpawner : InjectNetworkBehaviour, ISendCallbackListenerOnHostMigration
    {
        private readonly CompositeDisposable _disposables = new();
        
        private SpawnPositionHelper _spawnPositionHelper;
        private NetworkRunnerCallBacksListener _callBacksListener;
        
        [Networked] private PlayerFactory Factory { get; set; }
        [Networked] private PlayerRepository PlayerRepository { get; set; }

        [Inject] private async void Construct(
            AsyncDependenciesRepository asyncDependenciesRepository,
            NetworkRunnerCallBacksListener callBacksListener,
            SpawnPositionHelper spawnPositionHelper)
        {
            _callBacksListener = callBacksListener;
            _spawnPositionHelper = spawnPositionHelper;

            bool hasStateAuthority = await GetStateAuthorityAsync();
            if (hasStateAuthority == false)
            {
                EndInitialization();
                return;
            }
            
            Factory = await asyncDependenciesRepository.GetInstanceAsync<PlayerFactory>();
            PlayerRepository = await asyncDependenciesRepository.GetInstanceAsync<PlayerRepository>();
            EndInitialization();
        }

        protected override void OnSpawnMethod()
        {
            if (HasStateAuthority == false)
                return;
            
            _callBacksListener
                .OnPlayerJoinedSubject
                .Subscribe(createdData => 
                    TryCreatePlayer(createdData.Item1, createdData.Item2).Forget())
                .AddTo(_disposables);
            _callBacksListener
                .OnPlayerLeftSubject
                .Subscribe(leftPlayerData => 
                    TryDespawnPlayer(leftPlayerData.Item1, leftPlayerData.Item2).Forget())
                .AddTo(_disposables);
        }

        public override void Despawned(NetworkRunner runner, bool hasState) =>
            _disposables.Dispose();
        
        public void OnHostMigration(NetworkRunnerCallBacksListener generalNetworkObjectsRepository) => 
            _callBacksListener = generalNetworkObjectsRepository;

        private async UniTask TryDespawnPlayer(NetworkRunner runner, PlayerRef playerRef)
        {
            if (runner.IsServer == false)
                return;
            await InitializeTask;
            if (PlayerRepository.TryGetByPlayerRef(out Player player, playerRef)) 
                Factory.Despawn(player);
        }
        
        private async UniTask TryCreatePlayer(NetworkRunner runner, PlayerRef playerRef)
        {
            if (runner == false)
                return;
            if (runner.IsServer == false) 
                return;
            if (PlayerRepository.TryGetByPlayerRef(out Player _, playerRef))
                return;
            await InitializeTask;
            Factory.Create(_spawnPositionHelper.GetSpawnPosition(), playerRef).Forget();
        }
    }
}