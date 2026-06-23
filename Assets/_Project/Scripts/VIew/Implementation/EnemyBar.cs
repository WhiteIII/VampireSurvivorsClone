using _Project.Scripts.ViewModel.Implementation;
using R3;
using UnityEngine;

namespace _Project.Scripts.View.Implementation
{
    public class EnemyBar : Bar<EnemyBarViewModel>
    {
        private RectTransform _rectTransform;

        public void Release()
        {
            if (Disposable.IsDisposed == false)
                Disposable.Dispose();
            SetViewModel(null);
        }

        protected override void Awake() => 
            _rectTransform = GetComponent<RectTransform>();

        protected override void OnAwakeMethodIfViewModelIsNotNull()
        {
            base.OnAwakeMethodIfViewModelIsNotNull();
            ViewModel
                .OnPositionChanged
                .Subscribe(x => Move(x))
                .AddTo(Disposable);
        }

        protected override void OnSetViewModel()
        {
            base.OnSetViewModel();
            ViewModel
                .OnPositionChanged
                .Subscribe(x => Move(x))
                .AddTo(Disposable);
        }

        private void Move(Vector2 positionTo) => 
            _rectTransform.anchoredPosition = positionTo;
    }
}