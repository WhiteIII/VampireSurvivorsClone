using _Project.Scripts.Common.Services.Factories;
using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.View.Base;
using _Project.Scripts.View.Implementation;
using _Project.Scripts.VIew.Services.Factories.Implementation;
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
            BindAsync<EnemiesBarsViewModel, EnemiesBarsViewModelFactory>();
            AddAsyncWindowFactory<EnemiesBarsWindow, EnemiesBarsWindowFactory>();
        }

        private void AddAsyncWindowFactory<TContract, TFactory>()
            where TFactory : IFactory<UniTask<TContract>>
            where TContract : Window
        {
            Container.Bind<IAsyncDependenceProvider<TContract>>()
                .FromFactory<AddFactoryToUIController<TContract, TFactory>>().AsSingle();
        }
    }
}