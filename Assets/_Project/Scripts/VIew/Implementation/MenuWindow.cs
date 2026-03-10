using _Project.Scripts.VIew.Base;
using _Project.Scripts.ViewModel.Base;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.VIew.Implementation
{
    public class MenuWindow : Window<MenuViewModel>
    {
        [SerializeField] private Button _playButton;

        protected override void OnSetup() => 
            _playButton.OnClickAsObservable().Subscribe(_ => OnPlayButtonClick()).AddTo(this);

        private async void OnPlayButtonClick()
        {
            _playButton.interactable = false;
            await ViewModel.StartGameplay();
        }
    }
}