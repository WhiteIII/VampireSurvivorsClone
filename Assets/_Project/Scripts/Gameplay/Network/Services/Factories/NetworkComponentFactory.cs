using Fusion;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories
{
    public class NetworkComponentFactory<T> : IFactory<T>
        where T : NetworkBehaviour
    {
        private readonly NetworkObjectsCreator _creator;

        public NetworkComponentFactory(NetworkObjectsCreator creator) => 
            _creator = creator;

        public T Create() => 
            _creator.CreateEmptyNetworkObject<T>();
    }
}