using _Project.Scripts.Configs.Base;
using _Project.Scripts.Configs.Services.Base;
using _Project.Scripts.ViewModel.Base;
using Fusion;
using R3;

namespace _Project.Scripts.ViewModel.Implementation
{
    public partial class CreateGameOrConnectToGameViewModel : IViewModel
    {
        public Observable<StartGameArgs> StartGameObservable => _startGameSubject;

        private readonly Subject<StartGameArgs> _startGameSubject = new();
        private readonly IConfigService _configService;

        public CreateGameOrConnectToGameViewModel(IConfigService configService) => 
            _configService = configService;

        public void CreateGameOrConnectToGame(string sessionName, SessionMode sessionMode) =>
            _startGameSubject.OnNext(CreateStartGameArgs(sessionName, sessionMode));

        private StartGameArgs CreateStartGameArgs(string sessionName, SessionMode sessionMode)
        {
            StartGameArgs startGameArgs = new()
            {
                SessionName = sessionName
            };
            if (sessionMode == SessionMode.Host)
                startGameArgs.GameMode = GameMode.Host;
            else
                startGameArgs.GameMode = GameMode.Client;
            startGameArgs.PlayerCount = _configService.GetConfig<IGameConfig>().MaxPlayerCountInSession;
            return startGameArgs;
        }
    }

    public partial class CreateGameOrConnectToGameViewModel
    {
        public enum SessionMode
        {
            Host,
            Client
        }
    }
}