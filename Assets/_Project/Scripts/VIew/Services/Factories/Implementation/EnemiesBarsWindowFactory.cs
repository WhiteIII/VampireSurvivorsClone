using _Project.Scripts.Common.Services.Factories.Implementation;
using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.View.Implementation;
using _Project.Scripts.View.Services.Factories.Base;
using _Project.Scripts.ViewModel.Implementation;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.VIew.Services.Factories.Implementation
{
    public class EnemiesBarsWindowFactory : BaseWindowFactoryAsync<EnemiesBarsWindow>
    {
        private readonly IAsyncDependenciesRepository _asyncDependenciesRepository;
        
        public EnemiesBarsWindowFactory(
            [Inject(Id = "EnemiesBarsWindowAssetReference")]AssetReference assetReference,
            WindowsCreator windowCreator,
            IAsyncDependenciesRepository asyncDependenciesRepository) : base(windowCreator, assetReference)
        {
            _asyncDependenciesRepository = asyncDependenciesRepository;
        }

        public override async UniTask<EnemiesBarsWindow> Create()
        {
            EnemiesBarsViewModel enemiesBarsView = await _asyncDependenciesRepository
                .GetInstanceAsync<EnemiesBarsViewModel>();
            return CreateByCreator(enemiesBarsView);
        }
    }
}