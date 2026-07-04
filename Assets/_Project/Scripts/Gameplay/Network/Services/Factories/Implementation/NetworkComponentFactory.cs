using System.Collections.Generic;
using _Project.Scripts.Gameplay.Network.Services.Factories.Base;
using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Base;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Cysharp.Threading.Tasks;
using Fusion;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Implementation
{
    public class NetworkComponentFactory<T> : INetworkFactory<T>
        where T : NetworkBehaviour
    {
        private readonly NetworkCreatorForBinding _creator;
        private readonly NetworkTransform _parent;

        public NetworkComponentFactory(
            NetworkCreatorForBinding creator,
            [Inject(Id = "NetworkServicesParent")] NetworkTransform parent)
        {
            _creator = creator;
            _parent = parent;
        }

        public UniTask<T> Create() => 
            _creator.CreateEmptyNetworkObjectWithComponent<T>(_parent);

        public void Despawn(T item) => 
            _creator.Despawn(item);
    }
}