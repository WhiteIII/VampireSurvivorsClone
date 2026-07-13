using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.ViewModel.Implementation;
using _Project.Scripts.ViewModel.Services.Factories;
using Cysharp.Threading.Tasks;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public class UIGameplayInstaller : AdvancedMonoInstaller
    {
        public override void InstallBindings()
        {
            BindAsyncFromFactory<EnemiesBarsViewModel, EnemiesBarsViewModelFactory>();
        }
        
        private void BindAsyncFromFactory<TContract, TFactory>()
            where TFactory : IFactory<UniTask<TContract>>
            where TContract : class
        {
            Container.Bind<IAsyncDependence<object>>().To<AsyncDependence<TContract>>()
                .FromFactory<LayerAboveAsyncFactory<TContract, TFactory>>()
                .AsSingle().WhenInjectedInto<IAsyncDependenciesRepository>();
        }
    }
}