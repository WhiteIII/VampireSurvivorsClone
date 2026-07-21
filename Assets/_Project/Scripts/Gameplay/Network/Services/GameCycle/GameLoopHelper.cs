using System;
using _Project.Scripts.Common.Services.Initialize;
using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Cysharp.Threading.Tasks;
using R3;

namespace _Project.Scripts.Gameplay.Network.Services.GameCycle
{
    public class GameLoopHelper : IAsyncInitializable, IDisposable
    {
        private readonly NetworkObjectsCreatedInCompositionRootLocalRepository _repository;
        private readonly IAsyncDependenciesContainer _asyncDependenciesContainer;
        
        private readonly CompositeDisposable _disposable = new();
        
        public GameLoopHelper(
            NetworkObjectsCreatedInCompositionRootLocalRepository repository, 
            IAsyncDependenciesContainer asyncDependenciesContainer)
        {
            _repository = repository;
            _asyncDependenciesContainer = asyncDependenciesContainer;
        }

        public async UniTask InitializeAsync()
        {
            GameLoop gameLoop = await _asyncDependenciesContainer.Resolve<GameLoop>();
            _repository
                .OnAdd
                .Subscribe(x => gameLoop.TryRegister(x))
                .AddTo(_disposable);
        }

        public void Dispose() => 
            _disposable.Dispose();
    }
}