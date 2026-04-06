using System;
using Fusion;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.NetworkObjectProvider
{
    public class NetworkObjectProviderDecorator : INetworkObjectProvider
    {
        private readonly INetworkObjectProvider _networkObjectProvider;
        
        private bool _delayIfSceneManagerIsBusy = true; 
        
        public NetworkObjectAcquireResult AcquirePrefabInstance(
            NetworkRunner runner, 
            in NetworkPrefabAcquireContext context,
            out NetworkObject instance)
        {
            instance = null;

            if (_delayIfSceneManagerIsBusy && runner.SceneManager.IsBusy) {
                return NetworkObjectAcquireResult.Retry;
            }

            NetworkObject prefab;
            try {
                prefab = runner.Prefabs.Load(context.PrefabId, isSynchronous: context.IsSynchronous);
            } catch (Exception ex) {
                Log.Error($"Failed to load prefab: {ex}");
                return NetworkObjectAcquireResult.Failed;
            }

            if (!prefab) {
                // this is ok, as long as Fusion does not require the prefab to be loaded immediately;
                // if an instance for this prefab is still needed, this method will be called again next update
                return NetworkObjectAcquireResult.Retry;
            }

            //instance = InstantiatePrefab(runner, prefab);
            Assert.Check(instance);

            if (context.DontDestroyOnLoad) {
                runner.MakeDontDestroyOnLoad(instance.gameObject);
            } else {
                runner.MoveToRunnerScene(instance.gameObject);
            }

            runner.Prefabs.AddInstance(context.PrefabId);
            return NetworkObjectAcquireResult.Success;
            
            _networkObjectProvider.AcquirePrefabInstance(runner, context, out instance);
            //NetworkObjectProviderDefault
        }

        public void ReleaseInstance(NetworkRunner runner, in NetworkObjectReleaseContext context)
        {
            throw new System.NotImplementedException();
        }

        public NetworkPrefabId GetPrefabId(NetworkRunner runner, NetworkObjectGuid prefabGuid)
        {
            throw new System.NotImplementedException();
        }
    }

    public class NetworkObjectProviderDefaultWithInject : NetworkObjectProviderDefault
    {
        private IInstantiator _instantiator;

        [Inject] private void Construct(IInstantiator instantiator) =>
            _instantiator = instantiator;

        //protected override NetworkObject InstantiatePrefab(NetworkRunner runner, NetworkObject prefab) => 
        //    _instantiator.Instantiate<NetworkObject>(prefab.gameObject);
    }
}