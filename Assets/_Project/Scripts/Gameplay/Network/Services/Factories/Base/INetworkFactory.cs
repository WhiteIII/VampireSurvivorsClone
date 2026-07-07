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

    public interface INetworkFactory<TValue, in TParameter> : INetworkFactory, IFactory<TParameter, UniTask<TValue>>
    {
        void Despawn(TValue item);
    }
    
    public interface INetworkFactory<TValue, in TParameter1, in TParameter2> : INetworkFactory, 
        IFactory<TParameter1, TParameter2, UniTask<TValue>>
        where TValue : NetworkBehaviour
    {
        void Despawn(TValue item);
    }

    public interface INetworkFactory<TValue, in TParameter1, in TParameter2, in TParameter3> : INetworkFactory,
        IFactory<TParameter1, TParameter2, TParameter3, UniTask<TValue>>
        where TValue : NetworkBehaviour
    {
        void Despawn(TValue item);
    }
}