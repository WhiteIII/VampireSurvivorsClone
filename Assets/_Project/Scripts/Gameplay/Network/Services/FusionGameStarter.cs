using _Project.Scripts.Common.SceneSwitcher;
using _Project.Scripts.Gameplay.Network.Services.Factories.NetworkObjectProvider;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Gameplay.Network.Services
{
    public class FusionGameStarter
    {
        private const string GAMEPLAY = "Gameplay";

        private readonly NetworkRunner _networkRunner;
        private readonly NetworkObjectEndEmptyObjectProvider _networkObjectProvider;
        private readonly NetworkSceneManagerDefault _networkSceneManager;
        private readonly GameStateSwitcher _gameStateSwitcher;

        public bool GameStarted { get; private set; }
        
        public FusionGameStarter(
            GameStateSwitcher gameStateSwitcher, 
            NetworkRunner networkRunner,
            NetworkObjectEndEmptyObjectProvider networkObjectProvider, 
            NetworkSceneManagerDefault networkSceneManager)
        {
            _gameStateSwitcher = gameStateSwitcher;
            _networkRunner = networkRunner;
            _networkObjectProvider = networkObjectProvider;
            _networkSceneManager = networkSceneManager;
        }

        public async UniTask StartGameAsync()
        {
            NetworkSceneInfo networkSceneInfo = new();
            StartGameArgs startGameArgs = _gameStateSwitcher.StartGameArgs;

            networkSceneInfo.AddSceneRef(
                SceneRef.FromIndex(SceneUtility.GetBuildIndexByScenePath(GAMEPLAY)),
                LoadSceneMode.Additive);
            startGameArgs.SceneManager = _networkSceneManager;
            startGameArgs.ObjectProvider = _networkObjectProvider;
            startGameArgs.Scene = networkSceneInfo;
            
            await _networkRunner.StartGame(startGameArgs);
            GameStarted = true;
        }
    }
}