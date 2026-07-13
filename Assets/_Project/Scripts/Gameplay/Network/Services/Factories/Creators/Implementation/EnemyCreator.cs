using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Base;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Cysharp.Threading.Tasks;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Implementation
{
    public class EnemyCreator : NetworkLayerAboveObjectCreator<Enemy>
    {
        [Inject] private async UniTask Construct(
            IAsyncDependenciesRepository dependenciesRepository,
            IInstantiator instantiator) 
        {
            await GetStateAuthorityAsync();
            await Initialize(null, dependenciesRepository, instantiator);
            EndInitialization();
        }
    }
}