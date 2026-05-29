using Cysharp.Threading.Tasks;
using Fusion;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Base
{
    public interface INetworkFactory : IFactory
    {
    }

    public interface INetworkFactory<T> : INetworkFactory, IFactory<UniTask<T>>
        where T : NetworkBehaviour
    {
        void Despawn(T item);
    }

    public interface INetworkFactory<TValue, TParametor> : INetworkFactory, IFactory<TParametor, UniTask<TValue>>
    {
        void Despawn(TValue item);
    }
    
    public interface INetworkFactory<TValue, TParametor1, TParametor2> : INetworkFactory, 
        IFactory<TParametor1, TParametor2, UniTask<TValue>>
        where TValue : NetworkBehaviour
    {
        void Despawn(TValue item);
    }
}