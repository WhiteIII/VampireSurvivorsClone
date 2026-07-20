using System;
using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Common.Services.Factories.Base;
using _Project.Scripts.Common.Services.Repositories.Implementation;
using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network.Services.Factories.NetworkObjectProvider;
using _Project.Scripts.Gameplay.Network.Services.HostMigration;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Cysharp.Threading.Tasks;
using Fusion;
using R3;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Debug = System.Diagnostics.Debug;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Base
{
    public class NetworkObjectCreator<TBaseItem> : 
        INetworkObjectsCreator<TBaseItem>,
        IOnHostMigration,
        ISendGlobalRepositoryOnHostMigration
        where TBaseItem : NetworkBehaviour
    {
        public Observable<TBaseItem> OnSpawn => _onCreate;
        public Observable<TBaseItem> OnDespawn => _onDespawn;
        
        private LocalAssetProvider _localAssetProvider;
        private NetworkComponentCreationRepository _networkComponentCreationRepository;
        private NetworkRunner _networkRunner;
        private NetworkObjectEndEmptyObjectProvider _objectProvider;
        
        private readonly Subject<TBaseItem> _onCreate = new();
        private readonly Subject<TBaseItem> _onDespawn = new();

        public NetworkObjectCreator(
            LocalAssetProvider localAssetProvider,
            NetworkRunner networkRunner, 
            NetworkComponentCreationRepository networkComponentCreationRepository,
            NetworkObjectEndEmptyObjectProvider objectProvider)
        {
            _localAssetProvider = localAssetProvider;
            _networkComponentCreationRepository = networkComponentCreationRepository;
            _objectProvider = objectProvider;
            _networkRunner = networkRunner;
        }

        public void OnHostMigration(GeneralNetworkObjectsRepository generalNetworkObjectsRepository)
        {
            _networkRunner = generalNetworkObjectsRepository.CurrentNetworkRunner;
            _objectProvider = generalNetworkObjectsRepository.CurrentNetworkObjectProvider;
        }

        public void OnHostMigration(GlobalRepository globalRepository)
        {
            if (globalRepository.TryGet(out LocalAssetProvider assetProvider))
                _localAssetProvider = assetProvider;
            else
                throw new Exception("Asset provider not found!");
        }

        public UniTask<T> Create<T>(AssetReference assetReference, bool isWithInjection) where T : TBaseItem =>
            CreateWithParameters<T>(assetReference, isWithInjection);

        public UniTask<T> Create<T>(AssetReference assetReference, bool isWithInjection, Vector3 position) where T : TBaseItem => 
            CreateWithParameters<T>(assetReference, isWithInjection, position);

        public UniTask<T> CreateEmptyNetworkObjectWithComponent<T>(bool isWithInjection, NetworkTransform parent = null) where T : TBaseItem =>
            CreateEmptyObjectWithParameters<T>(parent, isWithInjection);
        
        public void Despawn(TBaseItem item) => 
            _networkRunner.Despawn(SendObjectOnDespawn(item).Object);

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
            await WaitWhileNetworkObjectInjectionInProgressAsync(spawnedObject);
            
            return SendObjectOnSpawn(spawnedObject.GetComponent<T>());
        }
        
        public async UniTask<T> CreateWithParameters<T>(
            AssetReference assetReference, 
            bool isWithInjection = true,
            Vector3? position = null, 
            Quaternion? rotation = null,
            PlayerRef? playerRef = null) where T : TBaseItem
        {
            if (_networkRunner.IsServer == false)
                throw new Exception("An attempt was made to create an network object that was not a server!");

            GameObject prefab = _localAssetProvider.GetAsset<GameObject>(assetReference);
            _objectProvider.AddObjectAndInjectionFlagPair(prefab.GetComponent<NetworkObject>(), isWithInjection);
            NetworkObject spawnedObject = await _networkRunner.SpawnAsync(
                prefab, 
                position, 
                rotation, 
                playerRef);
            await WaitWhileNetworkObjectInjectionInProgressAsync(spawnedObject);
            
            return SendObjectOnSpawn(spawnedObject.GetComponent<T>());
        }

        public async UniTask<T> CreateEmptyObjectWithParameters<T>(
            NetworkTransform parent = null,
            bool isWithInjection = true,
            Vector3? position = null, 
            Quaternion? rotation = null,
            PlayerRef? playerRef = null) where T : TBaseItem
        {
            if (_networkRunner.IsServer == false)
                throw new Exception("An attempt was made to create an network object that was not a server!");

            uint freeRawValue = _networkComponentCreationRepository.GetIdByType<T>();
            NetworkPrefabId networkPrefabId = NetworkPrefabId.FromRaw(freeRawValue);
            
            _objectProvider.AddTypeAndInjectionFlagPair(typeof(T), isWithInjection);
            NetworkObject spawnedObject = await _networkRunner.SpawnAsync(
                networkPrefabId, 
                position, 
                rotation, 
                playerRef);
            if (parent)
                spawnedObject.transform.SetParent(parent.transform);
            await WaitWhileNetworkObjectInjectionInProgressAsync(spawnedObject);
            
            return SendObjectOnSpawn(spawnedObject.GetComponent<T>());
        }
        
        private T SendObjectOnSpawn<T>(T instance)
            where T : TBaseItem
        {
            _onCreate.OnNext(instance);
            return instance;
        }

        private T SendObjectOnDespawn<T>(T instance)
            where T : TBaseItem
        {
            _onDespawn.OnNext(instance);
            return instance;
        }

        private async UniTask WaitWhileNetworkObjectInjectionInProgressAsync(NetworkObject networkObject)
        {
            foreach (NetworkBehaviour networkBehaviour in networkObject.NetworkedBehaviours)
            {
                if (networkBehaviour is InjectNetworkBehaviour injectNetworkBehaviour)
                    await injectNetworkBehaviour.InitializeTask;
            }
        }
    }
}