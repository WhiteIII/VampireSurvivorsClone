using System;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.ViewModel.Base
{
    public class MenuViewModel : IViewModel
    {
        private Func<UniTask> _onGoToGameplay;
        
        public void SetOnToGameplayMethod(Func<UniTask> onGoToGameplay) => 
            _onGoToGameplay = onGoToGameplay;
        
        public UniTask StartGameplay() => 
            _onGoToGameplay();
    }
}