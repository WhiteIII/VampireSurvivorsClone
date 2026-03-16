using System;
using _Project.Scripts.ViewModel.Base;
using Cysharp.Threading.Tasks;
using Fusion;
using R3;
using Zenject;

namespace _Project.Scripts.ViewModel.Implementation
{
    public class MenuViewModel : IViewModel, IInitializable, IDisposable
    {
        private readonly CreateGameOrConnectToGameViewModel _createGameOrConnectToGameViewModel;
        private readonly CompositeDisposable _disposables = new();
        
        private Func<StartGameArgs, UniTask> _onGoToGameplay;

        public MenuViewModel(CreateGameOrConnectToGameViewModel createGameOrConnectToGameViewModel) => 
            _createGameOrConnectToGameViewModel = createGameOrConnectToGameViewModel;

        public void Initialize() =>
            _createGameOrConnectToGameViewModel
                .StartGameObservable
                .Subscribe(StartGameplay)
                .AddTo(_disposables);

        public void Dispose() => 
            _disposables.Dispose();

        public void SetOnToGameplayMethod(Func<StartGameArgs, UniTask> onGoToGameplay) => 
            _onGoToGameplay = onGoToGameplay;

        private void StartGameplay(StartGameArgs startGameArgs) => 
            _onGoToGameplay(startGameArgs);
    }
}