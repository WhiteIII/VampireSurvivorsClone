using System;
using _Project.Scripts.Common.Services.Factories.Base;
using _Project.Scripts.CompositionRoot.Services;
using Fusion;
using R3;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services
{
    public class NetworkCreatorAdapter<T> : IInitializable, IDisposable
        where T : NetworkBehaviour
    {
        private readonly Action<NetworkBehaviour> _onSpawn;
        private readonly Action<NetworkBehaviour> _onDespawn;
        private readonly AsyncDependenciesRepository _asyncDependenciesRepository;

        private readonly CompositeDisposable _disposables = new();
        
        private INetworkObjectsCreator<T> _creator;

        public NetworkCreatorAdapter(
            Action<NetworkBehaviour> onSpawn,
            Action<NetworkBehaviour> onDespawn,
            AsyncDependenciesRepository asyncDependenciesRepository)
        {
            _onSpawn = onSpawn;
            _onDespawn = onDespawn;
            _asyncDependenciesRepository = asyncDependenciesRepository;
        }
        
        public async void Initialize()
        {
            _creator = await _asyncDependenciesRepository.GetInstanceAsync<INetworkObjectsCreator<T>>();
            _creator
                .OnSpawn
                .Subscribe(x => _onSpawn.Invoke(x))
                .AddTo(_disposables);
            _creator
                .OnDespawn
                .Subscribe(x => _onDespawn.Invoke(x))
                .AddTo(_disposables);
        }

        public void Dispose() => 
            _disposables.Dispose();
    }
}