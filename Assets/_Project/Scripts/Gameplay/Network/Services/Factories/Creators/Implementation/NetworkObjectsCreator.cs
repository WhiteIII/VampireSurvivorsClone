using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Base;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Fusion;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Implementation
{
    public class NetworkObjectsCreator : NetworkLayerAboveObjectCreator<NetworkBehaviour>
    {
        [Inject] private async void Construct(
            AsyncDependenciesRepository dependenciesRepository,
            IInstantiator instantiator) 
        {
            NetworkObjectsRepository repository = await dependenciesRepository.GetInstanceAsync<NetworkObjectsRepository>();
            await Initialize(repository, dependenciesRepository, instantiator);
            EndInitialization();
        }
    }
}