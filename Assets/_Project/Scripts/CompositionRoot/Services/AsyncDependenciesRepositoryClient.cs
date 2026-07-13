using System.Collections.Generic;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Cysharp.Threading.Tasks;
using Fusion;

namespace _Project.Scripts.CompositionRoot.Services
{
    public class AsyncDependenciesRepositoryClient : AsyncDependenciesRepository
    {
        private readonly NetworkBehavioursRepository _networkBehaviours;
        
        public AsyncDependenciesRepositoryClient(
            List<IAsyncDependence<object>> dependencies,
            NetworkBehavioursRepository networkBehaviours) : base(dependencies)
        {
            _networkBehaviours = networkBehaviours;
        }
        
        protected sealed override async UniTask OnInitialize()
        {
            await _networkBehaviours.InitializeAsync();
            foreach (NetworkBehaviour networkBehaviour in _networkBehaviours)
                Add(networkBehaviour);
        }
    }
}