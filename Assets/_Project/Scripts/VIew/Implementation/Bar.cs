using _Project.Scripts.View.Base;
using _Project.Scripts.ViewModel.Base;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.View.Implementation
{
    public class Bar<T> : Window<T>
        where T : IBarViewModel
    {
        [SerializeField] private Slider _slider;

        protected override void OnAwakeMethod()
        {
            ViewModel
                .OnValueChanged
                .Subscribe(x => _slider.value = x)
                .AddTo(this);
        }
    }
}