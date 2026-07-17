using System.Collections.Generic;
using _Project.Scripts.Common.Services.Initialize;
using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Cysharp.Threading.Tasks;
using Fusion;

namespace _Project.Scripts.Gameplay.Network.Services.GameCycle
{
    public class GameLoopLocalBuffer : IAsyncInitializable
    {
        private readonly List<IUpdatable> _updatables = new();
        private readonly NetworkBehavioursRepository _repository;

        public IEnumerable<IUpdatable> Updatables => _updatables;
        
        public GameLoopLocalBuffer(NetworkBehavioursRepository repository) => 
            _repository = repository;

        public async UniTask InitializeAsync()
        {
            await _repository.InitialisationTask;
            foreach (NetworkBehaviour instance in _repository)
            {
                if (instance is IUpdatable updatable)
                    _updatables.Add(updatable);
            }            
        }
    }
}