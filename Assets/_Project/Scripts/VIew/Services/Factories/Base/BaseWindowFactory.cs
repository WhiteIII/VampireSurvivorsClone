using _Project.Scripts.Common.Services.Factories.Base;
using _Project.Scripts.Common.Services.Factories.Implementation;
using _Project.Scripts.View.Base;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.View.Services.Factories.Base
{
    public class BaseWindowFactory<TWindow> : BaseObjectFactory<TWindow>
        where TWindow : Window
    {
        public BaseWindowFactory(
            AssetReference assetReference,
            WindowsCreator windowCreator) : base(assetReference, windowCreator)
        {
        }
    }
}
