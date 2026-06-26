using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.NetworkObjectProvider
{
    public class NetworkObjectEndEmptyObjectProvider : NetworkObjectProviderDefaultWithInject
    {
        private static NetworkObjectBaker _baker;
        private static NetworkObjectBaker Baker => _baker ??= new NetworkObjectBaker();

        private readonly Dictionary<NetworkPrefabId, Type> _rawValuesAndTypes = new();

        private NetworkServicesCreationHelper _networkServicesCreationHelper;

        public void SetNetworkServicesCreationHelper(NetworkServicesCreationHelper networkServicesCreationHelper) => 
            _networkServicesCreationHelper = networkServicesCreationHelper;

        public async UniTask<uint> GetFreeRawValue()
        {
            await _networkServicesCreationHelper.InitialisationTask;
            return _networkServicesCreationHelper.GetRawValue();  
        }

        public async UniTask AddRawValueAndType<T>(NetworkPrefabId networkPrefabId) 
            where T : NetworkBehaviour
        {
            if (NetworkRunner.IsServer == false)
                return;
            _rawValuesAndTypes.Add(networkPrefabId, typeof(T));
            await _networkServicesCreationHelper.InitialisationTask;
            _networkServicesCreationHelper.Add<T>(networkPrefabId);
        }

        public override NetworkObjectAcquireResult AcquirePrefabInstance(
            NetworkRunner runner, 
            in NetworkPrefabAcquireContext context,
            out NetworkObject instance)
        {
            instance = null;
            if (_networkServicesCreationHelper.IsReady == false)
                return NetworkObjectAcquireResult.Retry;

            if (_rawValuesAndTypes.Count != _networkServicesCreationHelper.NetworkPrefabIdCount)
            {
                _rawValuesAndTypes.Clear();
                _rawValuesAndTypes.AddRange(_networkServicesCreationHelper.GetNetworkPrefabIds());
            }
            
            if (_rawValuesAndTypes.TryGetValue(context.PrefabId, out var componentType))
            {
                if (componentType == null)
                {
                    Log.Error("An error occurred when adding a component to an empty object! Component type was null!");
                    instance = null;
                    return NetworkObjectAcquireResult.Failed;
                }

                GameObject emptyObject = CreateEmptyObject(componentType.Name);
                NetworkObject networkObject = AddComponentOnNetworkObject<NetworkObject>(emptyObject);
                AddComponentOnNetworkObject(emptyObject, componentType);
                Baker.Bake(emptyObject);
                
                if (context.DontDestroyOnLoad)
                    runner.MakeDontDestroyOnLoad(emptyObject);
                else
                    runner.MoveToRunnerScene(emptyObject);
                instance = networkObject;
                return NetworkObjectAcquireResult.Success;
            }

            return base.AcquirePrefabInstance(runner, context, out instance);
        }
    }
}