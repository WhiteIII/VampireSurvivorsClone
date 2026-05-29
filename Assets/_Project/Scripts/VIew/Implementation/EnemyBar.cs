using _Project.Scripts.ViewModel.Implementation;
using R3;
using UnityEngine;

namespace _Project.Scripts.View.Implementation
{
    public class EnemyBar : Bar<EnemyBarViewModel>
    {
        private RectTransform _rectTransform;

        protected override void OnAwakeMethod()
        {
            base.OnAwakeMethod();
            _rectTransform = GetComponent<RectTransform>();
            ViewModel
                .OnPositionChanged
                .Subscribe(x => Move(x))
                .AddTo(this);
        }
        
        private void Move(Vector2 positionTo) => 
            _rectTransform.anchoredPosition = positionTo;
    }
}