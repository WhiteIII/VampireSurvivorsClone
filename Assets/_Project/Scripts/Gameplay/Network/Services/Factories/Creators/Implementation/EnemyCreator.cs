using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Base;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Implementation
{
    public class EnemyCreator : NetworkLayerAboveObjectCreator<Enemy>
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
            
            EnemyRepository repository = await dependenciesRepository.GetInstanceAsync<EnemyRepository>();
            await Initialize(repository, dependenciesRepository, instantiator);
            EndInitialization();
        }
    }
}