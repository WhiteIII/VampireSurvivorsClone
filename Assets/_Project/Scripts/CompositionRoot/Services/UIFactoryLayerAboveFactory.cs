using _Project.Scripts.View.Base;
using _Project.Scripts.View.Services;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Services
{
    public class UIFactoryLayerAboveFactory<T> : IFactory<T>
        where T : IFactory<Window>
    {
        private readonly UIController _controller;
        private readonly IInstantiator _instantiator;
        
        public UIFactoryLayerAboveFactory(UIController controller, IInstantiator instantiator)
        {
            _controller = controller;
            _instantiator = instantiator;
        }

        public T Create() =>
            _controller.Add(_instantiator.Instantiate<T>());
    }
}