using _Project.Scripts.ViewModel.Base;

namespace _Project.Scripts.VIew.Implementation
{
    public class CreateGameWindow : StartGameWindow
    {
        protected override CreateGameOrConnectToGameViewModel.SessionMode SessionMode => CreateGameOrConnectToGameViewModel.SessionMode.Host;
    }
}