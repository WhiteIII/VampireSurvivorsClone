using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Fusion;

namespace _Project.Scripts.Gameplay.Network.Services.Factories
{
    public class NetworkServicesCreationHelper : NetworkBehaviour
    {
        public bool IsReady { get; private set; } = false;
        public int NetworkPrefabIdCount => NetworkPrefabId.Count;
        public UniTask InitialisationTask
        {
            get
            {
                if (IsReady)
                    return UniTask.CompletedTask;
                return UniTask.WaitWhile(() => IsReady == false);
            }
        }

        [Networked] private uint CurrentFreeRawValue { get; set; } = 10001;
        [Networked, Capacity(32)] private NetworkDictionary<NetworkPrefabId, NetworkString<_256>> NetworkPrefabId => default;

        public override void Spawned() => 
            IsReady = true;

        public Dictionary<NetworkPrefabId, Type> GetNetworkPrefabIds()
        {
            Dictionary<NetworkPrefabId, Type> networkPrefabIds = new();
            foreach (KeyValuePair<NetworkPrefabId, NetworkString<_256>> prefabIdAndTypeFullName in NetworkPrefabId)
                networkPrefabIds.Add(prefabIdAndTypeFullName.Key, Type.GetType(prefabIdAndTypeFullName.Value.ToString()));
            return networkPrefabIds;
        }
        
        public void Add<T>(NetworkPrefabId networkPrefabId) where T : NetworkBehaviour
        {
            if (HasStateAuthority == false)
                return;
            NetworkPrefabId.Add(networkPrefabId, new NetworkString<_256>(typeof(T).FullName));
        }
        
        public uint GetRawValue()
        {
            if (HasStateAuthority == false)
                Log.Error("Trying to get raw value from host!");
            CurrentFreeRawValue++;
            return CurrentFreeRawValue;
        }
    }
}