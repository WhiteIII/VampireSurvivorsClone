using _Project.Scripts.ViewModel.Implementation;

namespace _Project.Scripts.View.Implementation
{
    public class ConnectToGameWindow : StartGameWindow
    {
        protected override CreateGameOrConnectToGameViewModel.SessionMode SessionMode => CreateGameOrConnectToGameViewModel.SessionMode.Client;
    }
}