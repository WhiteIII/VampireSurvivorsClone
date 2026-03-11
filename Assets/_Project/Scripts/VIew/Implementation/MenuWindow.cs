using _Project.Scripts.View.Base;
using _Project.Scripts.View.Services;
using _Project.Scripts.View.Services.Repositrories;
using _Project.Scripts.ViewModel.Base;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _Project.Scripts.View.Implementation
{
    public class MenuWindow : Window<MenuViewModel>
    {
        [Header("Windows:")]
        [SerializeField] private CreateGameWindow _createGameWindow;
        [SerializeField] private ConnectToGameWindow _connectToGameWindow;
        
        [Header("Buttons:")]
        [SerializeField] private Button _createGameButton;
        [SerializeField] private Button _connectToGameButton;
        
        private WindowSwitcher _windowSwitcher;

        protected override void OnSetup()
        {
            _windowSwitcher = new WindowSwitcher(_createGameWindow, _connectToGameWindow);
            
            _createGameButton
                .OnClickAsObservable()
                .Subscribe(_ => SwitchToCreateGameWindow().Forget())
                .AddTo(this);
            _connectToGameButton
                .OnClickAsObservable()
                .Subscribe(_ => SwitchToConnectToGameWindow().Forget())
                .AddTo(this);
            
            _createGameWindow
                .OnCloseObservable
                .Subscribe(task => DisableInteractivityDuringAsync(task).Forget())
                .AddTo(this);
            _connectToGameWindow
                .OnCloseObservable
                .Subscribe(task => DisableInteractivityDuringAsync(task).Forget())
                .AddTo(this);
        }

        private UniTask SwitchToCreateGameWindow() =>
            DisableInteractivityDuringAsync(_windowSwitcher.Switch<ConnectToGameWindow, CreateGameWindow>());

        private UniTask SwitchToConnectToGameWindow() =>
            DisableInteractivityDuringAsync(_windowSwitcher.Switch<CreateGameWindow, ConnectToGameWindow>());

        private async UniTask DisableInteractivityDuringAsync(UniTask task)
        {
            DisableInteractable();
            await task;
            EnableInteractable();
        }
        
        protected override void OnEnableInteractable()
        {
            _connectToGameButton.interactable = true;
            _createGameButton.interactable = true;
        }

        protected override void OnDisableInteractable()
        {
            _connectToGameButton.interactable = false;
            _createGameButton.interactable = false;
        }
    }
}