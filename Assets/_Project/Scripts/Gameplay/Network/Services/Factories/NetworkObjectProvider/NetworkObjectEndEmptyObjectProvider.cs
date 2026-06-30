using System;
using System.Collections.Generic;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Cysharp.Threading.Tasks;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.NetworkObjectProvider
{
    public class NetworkObjectEndEmptyObjectProvider : NetworkObjectProviderDefaultWithInject
    {
        private static NetworkObjectBaker _baker;
        private static NetworkObjectBaker Baker => _baker ??= new NetworkObjectBaker();

        private NetworkComponentCreationRepository _networkComponentCreationRepository;

        [Inject] private void Construct(NetworkComponentCreationRepository networkComponentCreationRepository) =>
            _networkComponentCreationRepository = networkComponentCreationRepository;
        
        public override NetworkObjectAcquireResult AcquirePrefabInstance(
            NetworkRunner runner, 
            in NetworkPrefabAcquireContext context,
            out NetworkObject instance)
        {
            instance = null;
            if (_networkComponentCreationRepository.TryGetTypeById(context.PrefabId.RawValue, out Type componentType))
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