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
            instance = null;

            if (runner.SceneManager.IsBusy) 
                return NetworkObjectAcquireResult.Retry;
            NetworkObject prefab;

            if (_currentPrefabId != context.PrefabId.RawValue)
            {
                try
                {
                    prefab = runner.Prefabs.Load(context.PrefabId, isSynchronous: context.IsSynchronous);
                } 
                catch (Exception ex)
                {
                    Log.Error($"Failed to load prefab: {ex}");
                    return NetworkObjectAcquireResult.Failed;
                }
                if (prefab == false)
                    return NetworkObjectAcquireResult.Retry;
                instance = InstantiatePrefab(runner, prefab);
            }
            else 
            {
                if (_currentComponentType == null)
                {
                    Log.Error("An error occurred when adding a component to an empty object! Component type was null!");
                    return NetworkObjectAcquireResult.Failed;
                }
                GameObject emptyObject = CreateEmptyObject(_currentComponentType.Name);
                instance = AddComponentOnNetworkObject<NetworkObject>(emptyObject);
                AddComponentOnNetworkObject(emptyObject, _currentComponentType);
                Baker.Bake(emptyObject);
            }
            
            Assert.Check(instance);

            if (context.DontDestroyOnLoad)
                runner.MakeDontDestroyOnLoad(instance.gameObject);
            else
                runner.MoveToRunnerScene(instance.gameObject);

            runner.Prefabs.AddInstance(context.PrefabId);
            return NetworkObjectAcquireResult.Success;
        }
    }
}