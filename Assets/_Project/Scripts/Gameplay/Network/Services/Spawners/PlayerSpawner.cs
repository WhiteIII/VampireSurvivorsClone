using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using _Project.Scripts.Gameplay.Network.Services.Factories.Implementation;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Cysharp.Threading.Tasks;
using Fusion;
using R3;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Spawners
{
    public class PlayerSpawner : InjectNetworkBehaviour
    {
        private readonly CompositeDisposable _disposables = new();
        
        private SpawnPositionHelper _spawnPositionHelper;
        private PlayersInSessionData _playersInSessionData;
        
        [Networked] private PlayerFactory Factory { get; set; }
        [Networked] private PlayerRepository PlayerRepository { get; set; }

        [Inject] private async UniTask Construct(
            IAsyncDependenciesContainer asyncDependenciesRepository,
            PlayersInSessionData playersInSessionData,
            SpawnPositionHelper spawnPositionHelper)
        {
            _playersInSessionData = playersInSessionData;
            _spawnPositionHelper = spawnPositionHelper;

            bool hasStateAuthority = await GetStateAuthorityAsync();
            if (hasStateAuthority == false)
            {
                EndInitialization();
                return;
            }
            
            Factory = await asyncDependenciesRepository.Resolve<PlayerFactory>();
            PlayerRepository = await asyncDependenciesRepository.Resolve<PlayerRepository>();
            EndInitialization();
        }

        protected override void OnSpawnMethod()
        {
            if (HasStateAuthority == false)
                return;
            
            _playersInSessionData
                .OnPlayerJoinedSubject
                .Subscribe(createdData => 
                    TrySpawnPlayer(createdData.Item1, createdData.Item2).Forget())
                .AddTo(_disposables);
            _playersInSessionData
                .OnPlayerLeftSubject
                .Subscribe(leftPlayerData => 
                    TryDespawnPlayer(leftPlayerData.Item1, leftPlayerData.Item2).Forget())
                .AddTo(_disposables);
        }

        public override void Despawned(NetworkRunner runner, bool hasState) =>
            _disposables.Dispose();
        
        //public void OnHostMigration(NetworkRunnerCallBacksListener generalNetworkObjectsRepository) => 
            //_callBacksListener = generalNetworkObjectsRepository;

        private async UniTask TryDespawnPlayer(NetworkRunner runner, PlayerRef playerRef)
        {
            if (runner.IsServer == false)
                return;
            await InitializeTask;
            if (PlayerRepository.TryGetByPlayerRef(out Player player, playerRef)) 
                Factory.Despawn(player);
        }
        
        private async UniTask TrySpawnPlayer(NetworkRunner runner, PlayerRef playerRef)
        {
            if (runner.IsServer == false) 
                return;
            if (PlayerRepository.TryGetByPlayerRef(out Player _, playerRef))
                return;
            await InitializeTask; 
            await Factory.Create(_spawnPositionHelper.GetSpawnPosition(), playerRef, true);
        }
    }
}