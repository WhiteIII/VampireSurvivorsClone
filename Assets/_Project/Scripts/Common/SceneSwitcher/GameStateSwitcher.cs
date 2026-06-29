using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.SceneManagement.LoadSceneMode;
using static UnityEngine.SceneManagement.SceneManager;

namespace _Project.Scripts.Common.SceneSwitcher
{
    public class GameStateSwitcher
    {
        private const string GAMEPLAY = "Gameplay";
        private const string MENU = "Menu";

        private readonly GeneralNetworkObjectsRepository _repository;
        
        public StartGameArgs StartGameArgs { get; private set; }
        
        public GameStateSwitcher(GeneralNetworkObjectsRepository repository) => 
            _repository = repository;

        public async UniTask GoToMenu()
        {
            if (loadedSceneCount > 1)
            {
                await _repository.CurrentNetworkRunner.UnloadScene(GAMEPLAY);
                _repository.DestroyNetworkRunnerAndSceneManager();
            }
            await LoadSceneAsync(MENU, Additive);
        }

        public async UniTask GoToGameplay(StartGameArgs startGameArgs)
        {
            StartGameArgs = startGameArgs;
            if (loadedSceneCount > 1)
                await UnloadSceneAsync(MENU);
            await LoadSceneAsync(GAMEPLAY, Additive);
        }
    }
}