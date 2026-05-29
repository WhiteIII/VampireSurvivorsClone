using System;
using _Project.Scripts.ViewModel.Base;
using R3;

namespace _Project.Scripts.ViewModel.Implementation
{
    public class BarViewModel : IBarViewModel, IDisposable
    {
        public Observable<float> OnValueChanged => _onValueChanged;

        private ReadOnlyReactiveProperty<int> _onHealthChanged;
        private ReadOnlyReactiveProperty<int> _onMaxHealthChanged;
        
        private readonly Subject<float> _onValueChanged = new();
        private readonly CompositeDisposable _disposables = new();

        public void Dispose() => 
            _disposables.Dispose();

        public void SetHealthObservables(
            ReadOnlyReactiveProperty<int> onHealthChanged, 
            ReadOnlyReactiveProperty<int> onMaxHealthChanged)
        {
            _onHealthChanged = onHealthChanged;
            _onMaxHealthChanged = onMaxHealthChanged;

            _onHealthChanged.Subscribe(_ => ChangeValue()).AddTo(_disposables);
        }

        private void ChangeValue() =>
            _onValueChanged.OnNext((float)_onHealthChanged.CurrentValue / _onMaxHealthChanged.CurrentValue);
    }
}