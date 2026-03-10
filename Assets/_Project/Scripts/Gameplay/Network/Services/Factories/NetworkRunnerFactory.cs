using _Project.Scripts.Common.Repositories.Base;
using _Project.Scripts.Common.Services.Factories.Base;
using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Gameplay.Network.Services.Factories
{
    public class NetworkRunnerFactory : BaseMonoBehaviourObjectFactory<NetworkRunner>
    {
        private readonly NetworkRunnerCallBacksListener _callBacksListener;
        
        public NetworkRunnerFactory(
            NetworkRunnerCallBacksListener callBacksListener, 
            AssetReference networkRunnerPrefab, 
            LocalObjectCreator<MonoBehaviour> creator) : base(networkRunnerPrefab, creator)
        {
            _callBacksListener = callBacksListener;
        }

        public NetworkRunner Create()
        {
            NetworkRunner networkRunner = CreateByCreator();
            networkRunner.AddCallbacks(_callBacksListener);
            return networkRunner;
        }
    }
}