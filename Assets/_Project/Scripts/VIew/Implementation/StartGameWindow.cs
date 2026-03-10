using _Project.Scripts.VIew.Base;
using _Project.Scripts.ViewModel.Base;
using ModestTree;
using R3;
using UnityEngine;
using UnityEngine.UI;
using static _Project.Scripts.ViewModel.Base.CreateGameOrConnectToGameViewModel;

namespace _Project.Scripts.VIew.Implementation
{
    public abstract class StartGameWindow : Window<CreateGameOrConnectToGameViewModel>
    {
        [SerializeField] private InputField _sessionNameInputField;
        [SerializeField] private Button _startGameButton;
        
        protected abstract SessionMode SessionMode { get; }

        protected override void OnSetup() =>
            _startGameButton
                .OnClickAsObservable()
                .Where(_ => OnCheckNonEmptyText())
                .Subscribe(_ => ViewModel.CreateGameOrConnectToGame(_sessionNameInputField.text, SessionMode))
                .AddTo(this);

        private bool OnCheckNonEmptyText()
        {
            if (_sessionNameInputField.text.IsEmpty())
                return false;
            return true;
        }
    }
}