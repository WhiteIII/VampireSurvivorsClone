using _Project.Scripts.View.Base;
using _Project.Scripts.ViewModel.Implementation;
using Cysharp.Threading.Tasks;
using ModestTree;
using R3;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static _Project.Scripts.ViewModel.Implementation.CreateGameOrConnectToGameViewModel;

namespace _Project.Scripts.View.Implementation
{
    public abstract class StartGameWindow : Window<CreateGameOrConnectToGameViewModel>
    {
        public Observable<UniTask> OnCloseObservable => _onCloseSubject;
        
        [SerializeField] private TMP_InputField _sessionNameInputField;
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _closeWindowButton;
        
        private readonly Subject<UniTask> _onCloseSubject = new();
        
        protected abstract SessionMode SessionMode { get; }

        protected override void OnAwakeMethod()
        {
            _closeWindowButton
                .OnClickAsObservable()
                .Subscribe(_ => _onCloseSubject.OnNext(CloseAsync()))
                .AddTo(this);
            _startGameButton
                .OnClickAsObservable()
                .Where(_ => OnCheckNonEmptyText())
                .Subscribe(_ => ViewModel.CreateGameOrConnectToGame(_sessionNameInputField.text, SessionMode))
                .AddTo(this);
        }
        
        protected override void OnEnableInteractable()
        {
            _startGameButton.interactable = true;
            _sessionNameInputField.interactable = true;
            _closeWindowButton.interactable = true;
        }

        protected override void OnDisableInteractable()
        {
            _startGameButton.interactable = false;
            _sessionNameInputField.interactable = false;
            _closeWindowButton.interactable = false;
        }

        private bool OnCheckNonEmptyText()
        {
            if (_sessionNameInputField.text.IsEmpty())
                return false;
            return true;
        }
    }
}