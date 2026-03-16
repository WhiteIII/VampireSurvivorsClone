using _Project.Scripts.ViewModel.Implementation;

namespace _Project.Scripts.View.Implementation
{
    public class CreateGameWindow : StartGameWindow
    {
        protected override CreateGameOrConnectToGameViewModel.SessionMode SessionMode => CreateGameOrConnectToGameViewModel.SessionMode.Host;
    }
}