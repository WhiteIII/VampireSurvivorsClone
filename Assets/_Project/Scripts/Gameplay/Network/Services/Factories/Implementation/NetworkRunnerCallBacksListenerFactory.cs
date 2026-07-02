using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Fusion;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Implementation
{
    public class NetworkRunnerCallBacksListenerFactory : IFactory<NetworkRunnerCallBacksListener>
    {
        private readonly NetworkRunner _networkRunner;
        private readonly IInstantiator _instantiator;

        public NetworkRunnerCallBacksListenerFactory(
            NetworkRunner networkRunner,
            IInstantiator instantiator)
        {
            _networkRunner = networkRunner;
            _instantiator = instantiator;
        }

        public NetworkRunnerCallBacksListener Create()
        {
            NetworkRunnerCallBacksListener listener = _instantiator.Instantiate<NetworkRunnerCallBacksListener>();
            _networkRunner.AddCallbacks(listener);
            return listener;
        }
    }
}