using System;
using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Base;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Implementation
{
    public class CharacterCreator : NetworkLayerAboveObjectCreator<Character>
    {
        [Inject] private async void Construct(
            AsyncDependenciesRepository dependenciesRepository,
            IInstantiator instantiator) 
        {
            bool hasStateAuthority = await GetStateAuthorityAsync();
            if (hasStateAuthority == false)
            {
                await Initialize(null, dependenciesRepository, instantiator);
                EndInitialization();
                return;
            }
            
            //EnemyRepository repository = await dependenciesRepository.GetInstanceAsync<EnemyRepository>();
            //await Initialize(repository, dependenciesRepository, instantiator);
            EndInitialization();
        }

        public async UniTask<T> CreateWithView<T>(AssetReference assetReference, Vector3 position) 
            where T : Character
        {
            T character = await Create<T>(assetReference, position);
            if (character.TryGetComponent(out Health health))
            {
                
            }
            throw new NotImplementedException();
        }
    }
}