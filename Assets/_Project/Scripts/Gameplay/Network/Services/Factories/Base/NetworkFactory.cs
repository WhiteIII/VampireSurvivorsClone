using _Project.Scripts.Common.Services.Factories.Base;
using Fusion;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Base
{
    public abstract class NetworkFactory<TValue, TCreator> : NetworkBehaviour, IFactory
        where TValue : NetworkBehaviour
        where TCreator : INetworkObjectsCreator<TValue>
    {
        protected TCreator Creator { get; private set; }

        [Inject] private void Construct(TCreator creator) => 
            Creator = creator;
    }
}