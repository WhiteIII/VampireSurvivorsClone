using System;
using Cysharp.Threading.Tasks;
using R3;
using Zenject;

namespace _Project.Scripts.ViewModel.Base
{
    public class MenuViewModel : IViewModel, IInitializable, IDisposable
    {
        private readonly CreateGameOrConnectToGameViewModel _createGameOrConnectToGameViewModel;
        private readonly CompositeDisposable _disposables = new();
        
        private Func<UniTask> _onGoToGameplay;

        public MenuViewModel(CreateGameOrConnectToGameViewModel createGameOrConnectToGameViewModel) => 
            _createGameOrConnectToGameViewModel = createGameOrConnectToGameViewModel;

        public void Initialize()
        {
            _createGameOrConnectToGameViewModel
                .StartGameObservable
                .Subscribe(_ => StartGameplay())
                .AddTo(_disposables);
        }

        public void Dispose() => 
            _disposables.Dispose();

        public void SetOnToGameplayMethod(Func<UniTask> onGoToGameplay) => 
            _onGoToGameplay = onGoToGameplay;

        public UniTask StartGameplay() => 
            _onGoToGameplay();
    }
}