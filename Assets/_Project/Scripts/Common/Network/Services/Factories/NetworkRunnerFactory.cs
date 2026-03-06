using System;
using Fusion;
using Zenject;

namespace _Project.Scripts.Common.Network.Services.Factories
{
    public class NetworkRunnerFactory : PlaceholderFactory<NetworkRunner>
    {
        private readonly NetworkRunnerCallBacksListener _callBacksListener;

        public NetworkRunnerFactory(
            NetworkRunnerCallBacksListener callBacksListener, 
            NetworkPrefabRef networkPrefabReference)
        {
            _callBacksListener = callBacksListener;
        }

        public override NetworkRunner Create()
        {
            throw new NotImplementedException();
        }
    }
}
