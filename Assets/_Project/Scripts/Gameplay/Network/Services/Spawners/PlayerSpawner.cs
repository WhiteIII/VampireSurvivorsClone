using System;
using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using _Project.Scripts.Gameplay.Network.Services.Factories;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Fusion;
using R3;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Spawners
{
    public class PlayerSpawner : IInitializable, IDisposable
    {
        private readonly IFactory<Vector3, PlayerRef, Player> _factory;
        private readonly NetworkRunnerCallBacksListener _callBacksListener;
        private readonly SpawnPositionHelper _spawnPositionHelper;
        private readonly PlayerRepository _playerRepository;
        private readonly CompositeDisposable _disposables = new();

        public PlayerSpawner(
            IFactory<Vector3, PlayerRef, Player> factory, 
            NetworkRunnerCallBacksListener callBacksListener,
            SpawnPositionHelper spawnPositionHelper, 
            PlayerRepository playerRepository)
        {
            _factory = factory;
            _callBacksListener = callBacksListener;
            _spawnPositionHelper = spawnPositionHelper;
            _playerRepository = playerRepository;
        }

        public void Initialize() =>
            _callBacksListener
                .OnPlayerJoinedSubject
                .Subscribe(createdData => 
                    TryCreatePlayer(createdData.Item1, createdData.Item2))
                .AddTo(_disposables);

        private void TryCreatePlayer(NetworkRunner runner, PlayerRef playerRef)
        {
            if (runner.IsServer == false)
                return;
            _factory.Create(_spawnPositionHelper.GetSpawnPosition(), playerRef);
        }
        
        public void Dispose() => 
            _disposables.Dispose();
    }
}