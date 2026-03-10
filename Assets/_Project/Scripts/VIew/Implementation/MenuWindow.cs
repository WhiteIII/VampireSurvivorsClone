using _Project.Scripts.View.Base;
using _Project.Scripts.View.Services;
using _Project.Scripts.View.Services.Repositrories;
using _Project.Scripts.ViewModel.Base;
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
        private WindowsRepository _repository; // TODO циклическая зависимость, поправить
        
        [Inject] private void Construct(WindowsRepository repository) => 
            _repository = repository;
        
        protected override void OnSetup()
        {
            _windowSwitcher = new WindowSwitcher(_createGameWindow, _connectToGameWindow);
            
            _createGameButton.OnClickAsObservable().Subscribe().AddTo(this);
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

        private void RemoveInteractivityToAllWindows() => 
            _repository.DisableInteractableAllWindowsWithout(
                typeof(CreateGameWindow), 
                typeof(ConnectToGameWindow));

        private void ReturnInteractivityToAllWindows() =>
            _repository.EnableInteractableAllWindowsWithout(
                typeof(CreateGameWindow), 
                typeof(ConnectToGameWindow));
    }
}