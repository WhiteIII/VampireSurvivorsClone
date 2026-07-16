using System.Linq;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Services
{
    public class AsyncDependenciesContainerFactory : IFactory<AsyncDependenciesContainer>
    {
        private readonly AsyncDependenciesContainersFromManyScenes _containers;
        private readonly IInstantiator _instantiator;

        public AsyncDependenciesContainerFactory(
            AsyncDependenciesContainersFromManyScenes containers,
            IInstantiator instantiator)
        {
            _containers = containers;
            _instantiator = instantiator;
        }

        public AsyncDependenciesContainer Create() => 
            _containers.AddContainer(_instantiator.Instantiate<AsyncDependenciesContainer>());
    }
}