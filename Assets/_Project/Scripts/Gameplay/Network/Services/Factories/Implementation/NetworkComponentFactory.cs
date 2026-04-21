using _Project.Scripts.Gameplay.Network.Services.Factories.Base;
using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Base;
using Cysharp.Threading.Tasks;
using Fusion;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Implementation
{
    public class NetworkComponentFactory<T> : INetworkFactory<T>
        where T : NetworkBehaviour
    {
        private NetworkCreatorForBinding _creator;

        public NetworkComponentFactory(NetworkCreatorForBinding creator) => 
            _creator = creator;

        public UniTask<T> Create() => 
            _creator.CreateEmptyNetworkObjectWithComponent<T>();

        public void Despawn(T item) => 
            _creator.Despawn(item);
    }
}