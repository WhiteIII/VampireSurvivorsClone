using _Project.Scripts.Common.Repositories.Base;
using _Project.Scripts.Common.Services.Factories.Base;
using _Project.Scripts.Common.Services.Factories.Implementation;
using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories
{
    public class NetworkRunnerFactory : NetworkObjectFactory<NetworkRunner>
    {
        private readonly NetworkRunnerCallBacksListener _callBacksListener;
        
        public NetworkRunnerFactory(
            NetworkRunnerCallBacksListener callBacksListener, 
            [Inject(Id = "NetworkRunnerAssetReference")]AssetReference networkRunnerPrefab, 
            GeneralNetworkObjectsCreator creator) : base(networkRunnerPrefab, creator)
        {
            _callBacksListener = callBacksListener;
        }

        public override NetworkRunner Create()
        {
            NetworkRunner networkRunner = CreateByCreator();
            networkRunner.AddCallbacks(_callBacksListener);
            return networkRunner;
        }
    }
}