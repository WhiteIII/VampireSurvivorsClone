using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Base;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Cysharp.Threading.Tasks;
using Fusion;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Implementation
{
    public class NetworkObjectsCreator : NetworkLayerAboveObjectCreator<NetworkBehaviour>
    {
        [Inject] private async UniTask Construct(
            IAsyncDependenciesContainer dependenciesContainer,
            IInstantiator instantiator) 
        {
            bool hasStateAuthority = await GetStateAuthorityAsync();
            if (hasStateAuthority == false)
            {
                await Initialize(null, dependenciesContainer, instantiator);
                EndInitialization();
                return;
            }
            
            NetworkObjectsRepository repository = await dependenciesContainer.Resolve<NetworkObjectsRepository>();
            await Initialize(repository, dependenciesContainer, instantiator);
            EndInitialization();
        }
    }
}