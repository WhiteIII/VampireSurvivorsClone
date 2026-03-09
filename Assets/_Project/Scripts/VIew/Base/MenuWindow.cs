using _Project.Scripts.ViewModel.Base;
using UnityEngine;
using UnityEngine.UI;
using R3;

namespace _Project.Scripts.VIew.Base
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