using _Project.Scripts.Common.Services.Factories.Implementation;
using _Project.Scripts.View.Implementation;
using _Project.Scripts.View.Services.Factories.Base;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.VIew.Services.Factories.Implementation
{
    public class EnemiesBarsWindowFactory : BaseWindowFactory<EnemiesBarsWindow>
    {
        public EnemiesBarsWindowFactory(
            [Inject(Id = "EnemiesBarsWindowAssetReference")]AssetReference assetReference,
            WindowsCreator windowCreator) : base(assetReference, windowCreator)
        {
        }
    }
}