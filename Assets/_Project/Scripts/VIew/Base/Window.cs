using System.Threading;
using _Project.Scripts.ViewModel.Base;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.View.Base
{
    public abstract class Window : MonoBehaviour, IWindow
    {
        private IWindowAnimation _windowAnimation;
        
        public bool IsOpen { get; private set; }
        public bool IsInteractable { get; private set; }

        private void Awake() => 
            _windowAnimation = GetComponent<IWindowAnimation>();

        private void OnDestroy() => 
            OnDestroyMethod();

        public async UniTask OpenAsync()
        {
            CancellationToken cancellationToken = this.GetCancellationTokenOnDestroy();
            await OnOpenAnimationStartAsync(cancellationToken);
            await _windowAnimation.PlayOpenAnimationAsync(cancellationToken);
            await OnOpenAnimationEndAsync(cancellationToken);
            OnEnableInteractable();
            IsOpen = true;
        }

        public async UniTask CloseAsync()
        {
            OnDisableInteractable();
            CancellationToken cancellationToken = this.GetCancellationTokenOnDestroy();
            await OnCloseAnimationStartAsync(cancellationToken);
            await _windowAnimation.PlayCloseAnimationAsync(cancellationToken);
            IsOpen = false;
        }

        public void EnableInteractable()
        {
            OnEnableInteractable();
            IsInteractable = true;
        }

        public void DisableInteractable()
        {
            OnDisableInteractable();
            IsInteractable = false;
        }
        
        protected virtual void OnEnableInteractable() { }
        
        protected virtual void OnDisableInteractable() { }
        
        protected virtual void OnDestroyMethod() { }
        
        protected virtual UniTask OnCloseAnimationStartAsync(CancellationToken cancellationToken = default) => 
            UniTask.CompletedTask;
        
        protected virtual UniTask OnOpenAnimationStartAsync(CancellationToken cancellationToken = default) => 
            UniTask.CompletedTask;
        
        protected virtual UniTask OnOpenAnimationEndAsync(CancellationToken cancellationToken = default) =>
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