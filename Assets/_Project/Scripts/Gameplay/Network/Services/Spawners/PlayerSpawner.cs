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
        
        private PlayerFactory _factory;
        private SpawnPositionHelper _spawnPositionHelper;
        private PlayerRepository _playerRepository;
        private NetworkRunnerCallBacksListener _callBacksListener;

        [Inject] private async void Construct(
            AsyncDependenciesRepository asyncDependenciesRepository,
            NetworkRunnerCallBacksListener callBacksListener,
            SpawnPositionHelper spawnPositionHelper)
        {
            _callBacksListener = callBacksListener;
            _spawnPositionHelper = spawnPositionHelper;
            
            _factory = await asyncDependenciesRepository.GetInstanceAsync<PlayerFactory>();
            _playerRepository = await asyncDependenciesRepository.GetInstanceAsync<PlayerRepository>();
            EndInitialization();
        }

        protected override void OnSpawn()
        {
            _callBacksListener
                .OnPlayerJoinedSubject
                .Subscribe(createdData => 
                    TryCreatePlayer(createdData.Item1, createdData.Item2))
                .AddTo(_disposables);
            _callBacksListener
                .OnPlayerLeftSubject
                .Subscribe(leftPlayerData => 
                    TryDespawnPlayer(leftPlayerData.Item1, leftPlayerData.Item2))
                .AddTo(_disposables);
        }

        public override void Despawned(NetworkRunner runner, bool hasState) =>
            _disposables.Dispose();
        
        public void OnHostMigration(NetworkRunnerCallBacksListener generalNetworkObjectsRepository) => 
            _callBacksListener = generalNetworkObjectsRepository;

        private void TryDespawnPlayer(NetworkRunner runner, PlayerRef playerRef)
        {
            if (runner.IsServer == false)
                return;
            if (_playerRepository.TryGetByPlayerRef(out Player player, playerRef)) 
                _factory.Despawn(player);
        }
        
        private void TryCreatePlayer(NetworkRunner runner, PlayerRef playerRef)
        {
            if (runner.IsServer == false)
                return;
            _factory.Create(_spawnPositionHelper.GetSpawnPosition(), playerRef).Forget();
        }
    }
}