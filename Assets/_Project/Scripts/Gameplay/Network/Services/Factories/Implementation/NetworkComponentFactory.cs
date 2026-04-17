using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Implementation;
using Cysharp.Threading.Tasks;
using Fusion;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Implementation
{
    public class NetworkComponentFactory<T> : IFactory<UniTask<T>>
        where T : NetworkBehaviour
    {
        private readonly NetworkObjectsCreator _creator;

        public NetworkComponentFactory(NetworkObjectsCreator creator) => 
            _creator = creator;

        public UniTask<T> Create() => 
            _creator.CreateEmptyNetworkObjectWithComponent<T>();
    }
}