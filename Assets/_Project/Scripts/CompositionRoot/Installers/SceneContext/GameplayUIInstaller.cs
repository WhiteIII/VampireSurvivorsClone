using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.View.Base;
using _Project.Scripts.VIew.Services.Factories.Implementation;
using _Project.Scripts.ViewModel.Implementation;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public class GameplayUIInstaller : AdvancedMonoInstaller
    {
        public override void InstallBindings()
        {
            BindFactories();
            BindViewModels();
        }

        private void BindViewModels()
        {
            BindIsSingle<EnemiesBarsViewModel>();
        }
        
        private void BindFactories()
        {
            BindIsSingle<EnemyBarFactory>().WhenInjectedInto<EnemiesBarsWindowFactory>();
            BindUIFactory<EnemiesBarsWindowFactory>();
        }

        private ConcreteIdArgConditionCopyNonLazyBinder BindUIFactory<T>() where T : IFactory<Window> => 
            Container.Bind<IFactory<Window>>().To<T>().FromFactory<UIFactoryLayerAboveFactory<T>>().AsSingle();
    }
}