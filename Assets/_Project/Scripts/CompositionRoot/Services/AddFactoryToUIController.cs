using _Project.Scripts.Common.Services.Factories;
using _Project.Scripts.View.Base;
using _Project.Scripts.View.Services;
using Cysharp.Threading.Tasks;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Services
{
    public class AddFactoryToUIController<TWindow, TAsyncWindowFactory> : IFactory<IAbstractOverAsyncFactory<TWindow>>
        where TWindow : Window
        where TAsyncWindowFactory : IFactory<UniTask<TWindow>>
    {
        private readonly DiContainer _container;
        private readonly UIController _uiController;

        public AddFactoryToUIController(DiContainer container, UIController uiController)
        {
            _container = container;
            _uiController = uiController;
        }

        public IAbstractOverAsyncFactory<TWindow> Create()
        {
            if (_uiController.TryGetAsyncWindowFactory(out IAbstractOverAsyncFactory<TWindow> factory))
                return factory;
            DiContainer subContainer = _container.CreateSubContainer();
            subContainer.Bind<TAsyncWindowFactory>().AsSingle()
                .WhenInjectedInto<AbstractOverAsyncFactory<TAsyncWindowFactory, TWindow>>();
            return _uiController.AddAsyncWindowFactory(
                subContainer.Instantiate<AbstractOverAsyncFactory<TAsyncWindowFactory, TWindow>>());
        }
    }
}