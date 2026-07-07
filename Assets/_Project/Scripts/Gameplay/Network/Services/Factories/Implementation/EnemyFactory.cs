using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using _Project.Scripts.Gameplay.Network.Services.Factories.Base;
using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Implementation;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Implementation
{
    public class EnemyFactory : NetworkFactory<Enemy, EnemyCreator>, INetworkFactory<Enemy, Vector3>
    {
        private AssetReference _enemyAssetReference;
        
        [Inject] private async void Construct(
            [Inject(Id = "EnemyPrefabAssetReference")] AssetReference prefabAssetReference, 
            AsyncDependenciesRepository asyncDependenciesRepository)
        {
            _enemyAssetReference = prefabAssetReference;

            bool hasStateAuthority = await GetStateAuthorityAsync();
            if (hasStateAuthority == false)
            {
                EndInitialization();
                return;
            }
            
            EnemyCreator creator = await asyncDependenciesRepository.GetInstanceAsync<EnemyCreator>();
            Initialize(creator);
            EndInitialization();
        } 
        
        public UniTask<Enemy> Create(Vector3 spawnPosition) => 
            Creator.Create<Enemy>(_enemyAssetReference, true, spawnPosition);

        public void Despawn(Enemy item) => 
            Creator.Despawn(item);
    }
}