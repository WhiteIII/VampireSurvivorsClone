using _Project.Scripts.Common.Services.Repositories.Implementation;
using Zenject;

namespace _Project.Scripts.Common.Services.Factories.Implementation
{
    public class FactoryWithGlobalRepository<T> : IFactory<T>
    {
        private readonly IInstantiator _instantiator;
        private readonly GlobalRepository _globalRepository;

        public FactoryWithGlobalRepository(IInstantiator instantiator, GlobalRepository globalRepository)
        {
            _instantiator = instantiator;
            _globalRepository = globalRepository;
        }

        public T Create() =>
            _globalRepository.Add(_instantiator.Instantiate<T>());
    }
}