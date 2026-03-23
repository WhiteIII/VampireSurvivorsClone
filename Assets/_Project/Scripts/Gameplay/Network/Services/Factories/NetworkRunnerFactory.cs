using Fusion;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories
{
    public class NetworkRunnerFactory : GeneralNetworkObjectFactory<NetworkRunner>
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