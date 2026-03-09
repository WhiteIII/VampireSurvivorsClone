using _Project.Scripts.Common.SceneSwitcher;
using Fusion;
using Zenject;

namespace _Project.Scripts.CompositionRoot.EntryPoints
{
    public class MenuEntryPoint : IInitializable
    {
        private readonly NetworkRunner _networkRunner;
        private readonly NetworkSceneManagerDefault _networkSceneManagerPrefabRef;
        private readonly GameStateSwitcher _gameStateSwitcher;
        
        public MenuEntryPoint(NetworkRunner networkRunner, NetworkSceneManagerDefault networkSceneManagerPrefabRef)
        {
            _networkRunner = networkRunner;
            _networkSceneManagerPrefabRef = networkSceneManagerPrefabRef;
        }

        public void Initialize()
        {
        }
    }
}
