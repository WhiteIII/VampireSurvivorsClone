using System.Threading;
using _Project.Scripts.ViewModel.Base;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.View.Base
{
    public abstract class Window : MonoBehaviour, IWindow
    {
        private IWindowAnimation _windowAnimation;
        private CancellationToken _cancellationToken;
        
        public bool IsOpen { get; private set; }
        public bool IsInteractable { get; private set; }

        private void Awake()
        {
            _windowAnimation = GetComponent<IWindowAnimation>();
            _cancellationToken = this.GetCancellationTokenOnDestroy();
            OnAwakeMethod();
        }

        private void OnDestroy() => 
            OnDestroyMethod();

        public async UniTask OpenAsync()
        {
            await OnOpenAnimationStartAsync(_cancellationToken);
            await _windowAnimation.PlayOpenAnimationAsync(_cancellationToken);
            await OnOpenAnimationEndAsync(_cancellationToken);
            OnEnableInteractable();
            IsOpen = true;
        }

        public async UniTask CloseAsync()
        {
            OnDisableInteractable();
            await OnCloseAnimationStartAsync(_cancellationToken);
            await _windowAnimation.PlayCloseAnimationAsync(_cancellationToken);
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
        
        protected virtual void OnAwakeMethod() { }
        
        protected virtual UniTask OnCloseAnimationStartAsync(CancellationToken cancellationToken = default) => 
            UniTask.CompletedTask;
        
        protected virtual UniTask OnOpenAnimationStartAsync(CancellationToken cancellationToken = default) => 
            UniTask.CompletedTask;
        
        protected virtual UniTask OnOpenAnimationEndAsync(CancellationToken cancellationToken = default) =>
            UniTask.CompletedTask;
    }

    public abstract class Window<T> : Window
        where T : class, IViewModel
    {
        protected T ViewModel { get; private set; }

        [Inject] private void Construct([InjectOptional] T viewModel) =>
            ViewModel = viewModel;

        protected sealed override void OnAwakeMethod()
        {
            AwakeVirtual();
            if (ViewModel != null)
                OnAwakeMethodIfViewModelIsNotNull();
        }

        public void SetViewModel(T viewModel)
        {
            if (viewModel == null)
            {
                OnRelease();
                ViewModel = null;
                return;
            }

            ViewModel = viewModel;
            OnSetViewModel();
        }

        protected virtual void OnRelease() { }

        protected virtual void AwakeVirtual() { }

        protected virtual void OnAwakeMethodIfViewModelIsNotNull() { }

        protected virtual void OnSetViewModel() { }
    }
}