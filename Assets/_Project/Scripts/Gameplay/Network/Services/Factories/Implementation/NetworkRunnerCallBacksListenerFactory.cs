using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Implementation
{
    public class NetworkRunnerCallBacksListenerFactory : IFactory<NetworkRunnerCallBacksListener>
    {
        private readonly GeneralNetworkObjectsRepository _generalNetworkObjectsRepository;
        private readonly IInstantiator _instantiator;

        public NetworkRunnerCallBacksListenerFactory(
            GeneralNetworkObjectsRepository generalNetworkObjectsRepository,
            IInstantiator instantiator)
        {
            _generalNetworkObjectsRepository = generalNetworkObjectsRepository;
            _instantiator = instantiator;
        }

        public NetworkRunnerCallBacksListener Create()
        {
            NetworkRunnerCallBacksListener listener = _instantiator.Instantiate<NetworkRunnerCallBacksListener>();
            _generalNetworkObjectsRepository.CurrentNetworkRunner.AddCallbacks(listener);
            return listener;
        }
    }
}