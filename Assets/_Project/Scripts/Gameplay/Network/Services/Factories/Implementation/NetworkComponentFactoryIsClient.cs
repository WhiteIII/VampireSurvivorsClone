using System;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Cysharp.Threading.Tasks;
using Fusion;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Implementation
{
    public class NetworkComponentFactoryIsClient<T> : IFactory<UniTask<T>>
        where T : NetworkBehaviour
    {
        private readonly NetworkBehavioursRepository _repository;
        private readonly DiContainer _container;
        
        public NetworkComponentFactoryIsClient(NetworkBehavioursRepository repository, DiContainer container)
        {
            _repository = repository;
            _container = container;
        }

        public async UniTask<T> Create()
        {
            await _repository.InitialisationTask;
            if (_repository.TryGet(out T component))
            {
                _container.Inject(component);
                return component;
            }
            throw new Exception("Component not found!");
        }
    }
}