using _Project.Scripts.Common.SceneSwitcher;
using _Project.Scripts.View.Services.Repositrories;
using Cysharp.Threading.Tasks;
using Fusion;
using Zenject;

namespace _Project.Scripts.CompositionRoot.EntryPoints
{
    public class MenuEntryPoint : IInitializable
    {
        private readonly NetworkRunner _networkRunner;
        private readonly NetworkSceneManagerDefault _networkSceneManager;
        private readonly GameStateSwitcher _gameStateSwitcher;
        private readonly WindowsRepository _windowsRepository;
        
        public MenuEntryPoint(
            NetworkRunner networkRunner, 
            NetworkSceneManagerDefault networkSceneManager, 
            GameStateSwitcher gameStateSwitcher)
        {
            _networkRunner = networkRunner;
            _networkSceneManager = networkSceneManager;
            _gameStateSwitcher = gameStateSwitcher;
        }

        public void Initialize()
        {
            
        }

        private async UniTask OnMoveToGameplay()
        {
            
        } 
    }
}
