using System.Threading;
using _Project.Scripts.View.Base;
using _Project.Scripts.ViewModel.Implementation;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.View.Implementation
{
    public class LoadingWindow : Window<LoadingWindowViewModel>
    {
        [SerializeField] private Slider _loadingSlider;

        protected override void OnAwakeMethod() => 
            ViewModel.Progress.Subscribe(x => _loadingSlider.value = x).AddTo(this);

        protected override UniTask OnOpenAnimationStartAsync(CancellationToken cancellationToken = default)
        {
            ViewModel.ResetLoadingProgress();
            return UniTask.CompletedTask;
        }
    }
}