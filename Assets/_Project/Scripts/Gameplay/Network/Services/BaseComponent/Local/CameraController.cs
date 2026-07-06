using System;
using _Project.Scripts.Common.Services.Initialize;
using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Cysharp.Threading.Tasks;
using Fusion;
using R3;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Network.Services.BaseComponent.Local
{
    public class CameraController : IAsyncInitializable
    {
        private readonly Camera _camera;
        private readonly NetworkRunnerCallBacksListener _listener;
        private readonly AsyncDependenciesRepository _dependenciesRepository;
        
        private Player _player;
        
        public CameraController(
            Camera camera,
            NetworkRunnerCallBacksListener listener,
            AsyncDependenciesRepository dependenciesRepository)
        {
            _camera = camera;
            _listener = listener;
            _dependenciesRepository = dependenciesRepository;
        }

        public async UniTask InitializeAsync()
        { 
            (NetworkRunner _, PlayerRef playerRef) = await _listener.OnPlayerJoinedSubject.FirstAsync();
            PlayerRepository playerRepository = await _dependenciesRepository.GetInstanceAsync<PlayerRepository>();
            if (playerRepository.TryGetByPlayerRef(out Player player, playerRef))
                _player = player;
            else
                throw new Exception("Player not found!");
            SetCamera();
        }

        private void SetCamera() => 
            _camera.transform.SetParent(_player.transform);
    }
}