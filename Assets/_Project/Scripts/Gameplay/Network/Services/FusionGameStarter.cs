using _Project.Scripts.Common.SceneSwitcher;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine.SceneManagement;

namespace _Project.Scripts.Gameplay.Network.Services
{
    public class FusionGameStarter
    {
        private const string GAMEPLAY = "Gameplay";
        
        private readonly GeneralNetworkObjectsRepository  _generalNetworkObjectsRepository;
        private readonly GameStateSwitcher _gameStateSwitcher;
        
        public FusionGameStarter(
            GeneralNetworkObjectsRepository generalNetworkObjectsRepository, 
            GameStateSwitcher gameStateSwitcher)
        {
            _generalNetworkObjectsRepository = generalNetworkObjectsRepository;
            _gameStateSwitcher = gameStateSwitcher;
        }

        public async UniTask StartGameAsync()
        {
            NetworkSceneInfo networkSceneInfo = new();
            StartGameArgs startGameArgs = _gameStateSwitcher.StartGameArgs;

            networkSceneInfo.AddSceneRef(SceneRef.FromIndex(SceneUtility.GetBuildIndexByScenePath(GAMEPLAY)));
            startGameArgs.SceneManager = _generalNetworkObjectsRepository.CurrentNetworkSceneManager;
            startGameArgs.ObjectProvider = _generalNetworkObjectsRepository.CurrentNetworkObjectProvider;
            startGameArgs.Scene = networkSceneInfo;
            
            await _generalNetworkObjectsRepository.CurrentNetworkRunner.StartGame(startGameArgs);
        }
    }
}