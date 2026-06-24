using System;
using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Common.Services.Factories.Base;
using _Project.Scripts.Common.Services.Repositories.Implementation;
using _Project.Scripts.Gameplay.Network.Services.Factories.NetworkObjectProvider;
using _Project.Scripts.Gameplay.Network.Services.HostMigration;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Base
{
    public class NetworkObjectCreator<TBaseItem> : 
        INetworkObjectsCreator<TBaseItem>,
        IOnHostMigration,
        ISendGlobalRepositoryOnHostMigration
        where TBaseItem : NetworkBehaviour
    {
        private LocalAssetProvider _localAssetProvider;
        private NetworkObjectEndEmptyObjectProvider _networkObjectProvider;
        private NetworkRunner _networkRunner;
        
        public NetworkObjectCreator(
            LocalAssetProvider localAssetProvider,
            GeneralNetworkObjectsRepository generalNetworkObjectsRepository)
        {
            _localAssetProvider = localAssetProvider;
            _networkObjectProvider = generalNetworkObjectsRepository.CurrentNetworkObjectProvider;
            _networkRunner = generalNetworkObjectsRepository.CurrentNetworkRunner;
        }

        public void OnHostMigration(GeneralNetworkObjectsRepository generalNetworkObjectsRepository)
        {
            _networkRunner = generalNetworkObjectsRepository.CurrentNetworkRunner;
            _networkObjectProvider = generalNetworkObjectsRepository.CurrentNetworkObjectProvider;
        }

        public void OnHostMigration(GlobalRepository globalRepository)
        {
            if (globalRepository.TryGet(out LocalAssetProvider assetProvider))
                _localAssetProvider = assetProvider;
            else
                throw new Exception("Asset provider not found!");
        }
        
        public UniTask<T> Create<T>(AssetReference assetReference) where T : TBaseItem =>
            CreateWithParameters<T>(assetReference);

        public UniTask<T> Create<T>(AssetReference assetReference, Vector3 position) where T : TBaseItem => 
            CreateWithParameters<T>(assetReference, position);

        public UniTask<T> CreateEmptyNetworkObjectWithComponent<T>() where T : TBaseItem =>
            CreateEmptyObjectWithParameters<T>();
        
        public void Despawn(TBaseItem item) => 
            _networkRunner.Despawn(item.Object);

        public async UniTask<T> CreateWithParameters<T>(
            NetworkPrefabRef assetReference,
            Vector3? position = null, 
            Quaternion? rotation = null,
            PlayerRef? playerRef = null) where T : TBaseItem
        {
            if (_networkRunner.IsServer == false)
                throw new Exception("An attempt was made to create an network object that was not a server!");
            
            NetworkObject spawnedObject = await _networkRunner.SpawnAsync(
                assetReference, 
                position, 
                rotation, 
                playerRef);

            return spawnedObject.GetComponent<T>();
        }
        
        public async UniTask<T> CreateWithParameters<T>(
            AssetReference assetReference, 
            Vector3? position = null, 
            Quaternion? rotation = null,
            PlayerRef? playerRef = null) where T : TBaseItem
        {
            if (_networkRunner.IsServer == false)
                throw new Exception("An attempt was made to create an network object that was not a server!");
            
            NetworkObject spawnedObject = await _networkRunner.SpawnAsync(
                _localAssetProvider.GetAsset<GameObject>(assetReference), 
                position, 
                rotation, 
                playerRef);

            return spawnedObject.GetComponent<T>();
        }

        public async UniTask<T> CreateEmptyObjectWithParameters<T>(
            Vector3? position = null, 
            Quaternion? rotation = null,
            PlayerRef? playerRef = null) where T : TBaseItem
        {
            if (_networkRunner.IsServer == false)
                throw new Exception("An attempt was made to create an network object that was not a server!");
            
            uint freeRawValue = await _networkObjectProvider.GetFreeRawValue();
            NetworkPrefabId networkPrefabId = NetworkPrefabId.FromRaw(freeRawValue);
            await _networkObjectProvider.AddRawValueAndType<T>(networkPrefabId);

            NetworkObject spawnedObject = await _networkRunner.SpawnAsync(
                networkPrefabId, 
                position, 
                rotation, 
                playerRef);
            
            return spawnedObject.GetComponent<T>();
        }
    }
}