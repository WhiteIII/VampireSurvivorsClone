using Fusion;
using Zenject;

namespace _Project.Scripts.CompositionRoot.EntryPoints
{
    public abstract class BaseNetworkSceneEntryPoint : IInitializable
    {
        private readonly NetworkRunner _networkRunner;

        protected bool IsServer => _networkRunner.IsServer;
        
        protected BaseNetworkSceneEntryPoint(NetworkRunner networkRunner) => 
            _networkRunner = networkRunner;

        public abstract void Initialize();
    }
}