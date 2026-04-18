using _Project.Scripts.Common.Services.Initialize;
using _Project.Scripts.View.Implementation;
using _Project.Scripts.View.Services;
using _Project.Scripts.ViewModel.Implementation;
using Zenject;

namespace _Project.Scripts.CompositionRoot.EntryPoints
{
    public class GameplayEntryPoint : IInitializable
    {
        private readonly AsyncInitializableRepository _initializableRepository;
        private readonly UIController _uiController;
        private readonly LoadingWindowViewModel _loadingWindowViewModel;
        
        public GameplayEntryPoint(
            AsyncInitializableRepository initializableRepository,
            UIController uiController,
            LoadingWindowViewModel loadingWindowViewModel)
        {
            _initializableRepository = initializableRepository;
            _uiController = uiController;
            _loadingWindowViewModel = loadingWindowViewModel;
        }

        public async void Initialize()
        {
            await _loadingWindowViewModel.StartLoadingAsync(_initializableRepository.GetTasks());
            await _uiController.CloseWindowAsync<LoadingWindow>();
        }
    }
}