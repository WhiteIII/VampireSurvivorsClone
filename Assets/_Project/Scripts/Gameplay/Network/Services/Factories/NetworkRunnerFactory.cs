using _Project.Scripts.Gameplay.Services.Factories.Base;
using Fusion;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories
{
    public class NetworkRunnerFactory : IFactory<NetworkRunner>
    {
        private readonly NetworkRunnerCallBacksListener _callBacksListener;
        private readonly AssetReference _networkRunnerPrefab;
        private readonly LocalObjectCreator _creator;
        
        public NetworkRunnerFactory(
            NetworkRunnerCallBacksListener callBacksListener, 
            AssetReference networkRunnerPrefab, 
            LocalObjectCreator creator)
        {
            _callBacksListener = callBacksListener;
            _networkRunnerPrefab = networkRunnerPrefab;
            _creator = creator;
        }

        public NetworkRunner Create()
        {
            NetworkRunner networkRunner = _creator.Create<NetworkRunner>(_networkRunnerPrefab);
            networkRunner.AddCallbacks(_callBacksListener);
            return networkRunner;
        }
    }
}