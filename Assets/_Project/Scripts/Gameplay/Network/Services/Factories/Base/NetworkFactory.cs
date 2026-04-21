using _Project.Scripts.Common.Services.Factories.Base;
using _Project.Scripts.CompositionRoot.Services;
using Fusion;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Base
{
    public abstract class NetworkFactory<TValue, TCreator> : InjectNetworkBehaviour, INetworkFactory
        where TValue : NetworkBehaviour
        where TCreator : INetworkObjectsCreator<TValue>
    {
        protected TCreator Creator { get; private set; }

        protected void Initialize(TCreator creator) => 
            Creator = creator;
    }
}