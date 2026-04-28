using System;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.NetworkObjectProvider
{
    public class NetworkObjectEndEmptyObjectProvider : NetworkObjectProviderDefaultWithInject
    {
        private static NetworkObjectBaker _baker;
        private static NetworkObjectBaker Baker => _baker ??= new NetworkObjectBaker();
        
        private uint _currentPrefabId;
        private Type _currentComponentType;
        
        public bool EmptyObjectCreationInProcess { get; private set; }
        
        public void SetPrefabIdAndComponentType<T>(uint id)
            where T : NetworkBehaviour
        {
            _currentPrefabId = id;
            _currentComponentType = typeof(T);
            EmptyObjectCreationInProcess = true;
        }

        public void ResetCreationEmptyObjectProcess()
        {
            _currentPrefabId = 0;
            _currentComponentType = null;
            EmptyObjectCreationInProcess = false;
        }
        
        public override NetworkObjectAcquireResult AcquirePrefabInstance(
            NetworkRunner runner, 
            in NetworkPrefabAcquireContext context,
            out NetworkObject instance)
        {
            if (_currentPrefabId == context.PrefabId.RawValue)
            {
                if (_currentComponentType == null)
                {
                    Log.Error("An error occurred when adding a component to an empty object! Component type was null!");
                    instance = null;
                    return NetworkObjectAcquireResult.Failed;
                }

                GameObject emptyObject = CreateEmptyObject(_currentComponentType.Name);
                NetworkObject networkObject = AddComponentOnNetworkObject<NetworkObject>(emptyObject);
                AddComponentOnNetworkObject(emptyObject, _currentComponentType);
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