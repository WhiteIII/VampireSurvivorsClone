using System.Threading;
using _Project.Scripts.ViewModel.Base;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.VIew.Base
{
    public abstract class Window : MonoBehaviour, IWindow
    {
        private IWindowAnimation _windowAnimation;
        
        private void Awake() => 
            _windowAnimation = GetComponent<IWindowAnimation>();

        private void OnDestroy() => 
            OnDestroyMethod();

        public async UniTask OpenAsync()
        {
            await OnOpenAnimationStartAsync(this.GetCancellationTokenOnDestroy());
            await _windowAnimation.PlayOpenAnimationAsync(this.GetCancellationTokenOnDestroy());
        }

        public async UniTask CloseAsync()
        {
            await OnCloseAnimationStartAsync(this.GetCancellationTokenOnDestroy());
            await _windowAnimation.PlayCloseAnimationAsync(this.GetCancellationTokenOnDestroy());
        }

        protected virtual void OnDestroyMethod() { }
        
        protected virtual UniTask OnCloseAnimationStartAsync(CancellationToken cancellationToken = default) => 
            UniTask.CompletedTask;
        
        protected virtual UniTask OnOpenAnimationStartAsync(CancellationToken cancellationToken = default) => 
            UniTask.CompletedTask;
    }

    public abstract class Window<T> : Window 
        where T : IViewModel
    {
        protected T ViewModel { get; private set; }

        public void Setup(T viewModel)
        {
            ViewModel = viewModel;
            OnSetup();
        }
        
        protected virtual void OnSetup() { }
    }
}
