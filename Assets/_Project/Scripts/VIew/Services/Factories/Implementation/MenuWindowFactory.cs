using _Project.Scripts.Common.Services.Factories.Implementation;
using _Project.Scripts.View.Implementation;
using _Project.Scripts.View.Services.Factories.Base;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.VIew.Services.Factories.Implementation
{
    public class MenuWindowFactory : BaseWindowFactory<MenuWindow>
    {
        public MenuWindowFactory(
            [Inject(Id = "MenuWindowAssetReference")]AssetReference assetReference, 
            WindowsCreator creator) : base(assetReference, creator)
        {
        }
    }
}