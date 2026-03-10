using _Project.Scripts.ViewModel.Base;

namespace _Project.Scripts.VIew.Implementation
{
    public class ConnectToGameWindow : StartGameWindow
    {
        protected override CreateGameOrConnectToGameViewModel.SessionMode SessionMode => CreateGameOrConnectToGameViewModel.SessionMode.Client;
    }
}