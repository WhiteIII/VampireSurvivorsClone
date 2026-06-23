using _Project.Scripts.View.Base;
using _Project.Scripts.ViewModel.Base;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.View.Implementation
{
    public class Bar<T> : Window<T>
        where T : class, IBarViewModel
    {
        [SerializeField] private Slider _slider;

        protected CompositeDisposable Disposable { get; private set; } = new();
        
        protected override void OnAwakeMethodIfViewModelIsNotNull()
        {
            ViewModel
                .OnValueChanged
                .Subscribe(x => _slider.value = x)
                .AddTo(Disposable);
        }
        
        protected override void OnSetViewModel()
        {
            if (Disposable.IsDisposed == false)
                Disposable.Dispose();
            Disposable = new CompositeDisposable();
            ViewModel
                .OnValueChanged
                .Subscribe(x => _slider.value = x)
                .AddTo(Disposable);
        }
    }
}