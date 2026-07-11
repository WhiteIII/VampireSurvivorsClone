using _Project.Scripts.Common.Services.Factories.Implementation;
using _Project.Scripts.View.Base;
using _Project.Scripts.ViewModel.Base;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.View.Services.Factories.Base
{
    public abstract class BaseWindowFactoryAsync<TWindow> : IFactory<UniTask<TWindow>>
        where TWindow : Window
    {
        private readonly WindowsCreator _windowCreator;
        private readonly AssetReference _assetReference;

        protected BaseWindowFactoryAsync(WindowsCreator windowCreator, AssetReference assetReference)
        {
            _windowCreator = windowCreator;
            _assetReference = assetReference;
        }

        public abstract UniTask<TWindow> Create();

        protected TWindow CreateByCreator<TViewModel>(TViewModel viewModel) where TViewModel : IViewModel => 
            _windowCreator.Create<TWindow, TViewModel>(_assetReference, viewModel);
    }
}