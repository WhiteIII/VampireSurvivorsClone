using _Project.Scripts.Common.Services.Factories.Base;
using _Project.Scripts.Common.Services.Repositories.Base;
using _Project.Scripts.Common.Services.Repositories.Implementation;
using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network.Services.GameCycle;
using _Project.Scripts.Gameplay.Network.Services.HostMigration;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Base
{
    public abstract class NetworkLayerAboveObjectCreator<TBaseItem> : 
        InjectNetworkBehaviour, 
        INetworkObjectsCreator<TBaseItem>,
        IOnHostMigration, 
        ISendGlobalRepositoryOnHostMigration
        where TBaseItem : NetworkBehaviour
    {
        private IRepository<TBaseItem> _repository;
        private NetworkObjectCreator<TBaseItem> _networkObjectCreator;
        
        [Networked] private GameLoop GameLoop { get; set; }
        [Networked] private NetworkBehaviour RepositoryNetworkBehaviour { get; set; }
        
        protected async UniTask Initialize(
            IRepository<TBaseItem> repository,
            AsyncDependenciesRepository asyncDependenciesRepository,
            IInstantiator instantiator)
        {
            _networkObjectCreator = instantiator.Instantiate<NetworkObjectCreator<TBaseItem>>();
            
            bool hasStateAuthority = await GetStateAuthorityAsync();
            if (hasStateAuthority == false)
            {
                _repository = (IRepository<TBaseItem>)RepositoryNetworkBehaviour;
                return;
            }
            
            _repository = repository;
            RepositoryNetworkBehaviour = (NetworkBehaviour)_repository;
            GameLoop = await asyncDependenciesRepository.GetInstanceAsync<GameLoop>();
        }

        public void OnHostMigration(GlobalRepository globalRepository) => 
            _networkObjectCreator.OnHostMigration(globalRepository);

        public void OnHostMigration(GeneralNetworkObjectsRepository generalNetworkObjectsRepository) =>
            _networkObjectCreator.OnHostMigration(generalNetworkObjectsRepository);
        
        public UniTask<T> Create<T>(AssetReference assetReference) where T : TBaseItem =>
            CreateWithParameters<T>(assetReference);

        public UniTask<T> Create<T>(AssetReference assetReference, Vector3 position) where T : TBaseItem => 
            CreateWithParameters<T>(assetReference, position);

        public UniTask<T> Create<T>(NetworkPrefabRef assetReference, Vector3 position) where T : TBaseItem =>
            CreateWithParameters<T>(assetReference, position);
        
        public UniTask<T> CreateEmptyNetworkObjectWithComponent<T>() where T : TBaseItem =>
            CreateEmptyObjectWithParameters<T>();

        public void Despawn(TBaseItem item)
        {
            GameLoop.TryUnregister(item);
            if (_repository.TryGet(out TBaseItem _))
                _repository.Remove(item);
            _networkObjectCreator.Despawn(item);
        }

        protected async UniTask<T> CreateWithParameters<T>(
            NetworkPrefabRef assetReference,
            Vector3? position = null,
            Quaternion? rotation = null,
            PlayerRef? playerRef = null) where T : TBaseItem
        {
            await InitializeTask;
            T spawnedObject = await _networkObjectCreator
                .CreateWithParameters<T>(assetReference, position, rotation, playerRef);
            return GameLoop.TryRegister(_repository.Add(spawnedObject));
        }
        
        protected async UniTask<T> CreateWithParameters<T>(
            AssetReference assetReference, 
            Vector3? position = null, 
            Quaternion? rotation = null,
            PlayerRef? playerRef = null) where T : TBaseItem
        {
            await InitializeTask;
            T spawnedObject = await _networkObjectCreator
                .CreateWithParameters<T>(assetReference, position, rotation, playerRef);
            return GameLoop.TryRegister(_repository.Add(spawnedObject));
        }

        protected async UniTask<T> CreateEmptyObjectWithParameters<T>(
            Vector3? position = null, 
            Quaternion? rotation = null,
            PlayerRef? playerRef = null) where T : TBaseItem
        {
            await InitializeTask;
            T spawnedObject = await _networkObjectCreator
                .CreateEmptyObjectWithParameters<T>(position, rotation, playerRef);
            return GameLoop.TryRegister(_repository.Add(spawnedObject));
        }
    }
}