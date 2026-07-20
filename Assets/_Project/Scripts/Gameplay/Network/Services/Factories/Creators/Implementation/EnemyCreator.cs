using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Base;
using Cysharp.Threading.Tasks;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Implementation
{
    public class EnemyCreator : NetworkLayerAboveObjectCreator<Enemy>
    {
        [Inject] private async UniTask Construct(
            IAsyncDependenciesContainer dependenciesContainer,
            IInstantiator instantiator) 
        {
            await GetStateAuthorityAsync();
            await Initialize(null, dependenciesContainer, instantiator);
            EndInitialization();
        }
    }
}