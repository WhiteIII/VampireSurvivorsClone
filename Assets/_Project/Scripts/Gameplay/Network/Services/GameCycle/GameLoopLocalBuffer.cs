using System.Collections.Generic;
using _Project.Scripts.Common.Services.Initialize;
using _Project.Scripts.CompositionRoot.Services;
using Cysharp.Threading.Tasks;
using Fusion;

namespace _Project.Scripts.Gameplay.Network.Services.GameCycle
{
    public class GameLoopLocalBuffer : IAsyncInitializable
    {
        private readonly List<IUpdatable> _updatables = new();
        private readonly IAsyncDependenciesRepository _dependencies;

        public IEnumerable<IUpdatable> Updatables => _updatables;
        
        public GameLoopLocalBuffer(IAsyncDependenciesRepository dependencies) => 
            _dependencies = dependencies;

        public async UniTask InitializeAsync()
        {
            await UniTask.WaitWhile(() => _dependencies.IsInitialized == false);
            foreach (object instance in _dependencies.Instances)
            {
                if (instance is IUpdatable updatable)
                    _updatables.Add(updatable);
            }            
        }
    }
}