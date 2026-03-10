using Fusion;
using R3;

namespace _Project.Scripts.ViewModel.Base
{
    public partial class CreateGameOrConnectToGameViewModel : IViewModel
    {
        public Observable<StartGameArgs> StartGameObservable => _startGameSubject;

        private readonly Subject<StartGameArgs> _startGameSubject = new();

        public void CreateGameOrConnectToGame(string sessionName, SessionMode sessionMode) =>
            _startGameSubject.OnNext(CreateStartGameArgs(sessionName, sessionMode));

        private StartGameArgs CreateStartGameArgs(string sessionName, SessionMode sessionMode)
        {
            StartGameArgs startGameArgs = new();
            startGameArgs.SessionName = sessionName;
            if (sessionMode == SessionMode.Host)
                startGameArgs.GameMode = GameMode.Host;
            else
                startGameArgs.GameMode = GameMode.Client;
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