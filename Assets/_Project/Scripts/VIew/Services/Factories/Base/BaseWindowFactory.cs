using _Project.Scripts.Common.Services.Factories.Base;
using _Project.Scripts.View.Base;
using _Project.Scripts.ViewModel.Base;
using UnityEngine.AddressableAssets;
using Zenject;

public class BaseWindowFactory<TWindow, TViewModel> : PlaceholderFactory<TWindow>
    where TWindow : Window<TViewModel>
    where TViewModel : IViewModel
{
    private readonly WindowsCreator _windowCreator;
    private readonly AssetReference _windowAssetReference;

    public BaseWindowFactory(
        WindowsCreator windowCreator,
        AssetReference windowAssetReference,
        TViewModel viewModel)
    {
        _windowCreator = windowCreator;
        _windowAssetReference = windowAssetReference;
        ViewModel = viewModel;
    }

    protected TViewModel ViewModel { get; }

    public override TWindow Create() => 
        SetupWindow(CreateByCreator());

    protected TWindow SetupWindow(TWindow window)
    {
        window.Setup(ViewModel);
        return window;
    }

    protected TWindow CreateByCreator() => 
        _windowCreator.Create<TWindow>(_windowAssetReference);
}
