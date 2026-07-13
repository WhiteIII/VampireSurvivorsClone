using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Fusion;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Services
{
    public class GeneralAsyncDependenciesRepository : IAsyncDependenciesRepository
    {
        private readonly AsyncDependenciesRepository _dependenciesRepositoryHost;
        private readonly AsyncDependenciesRepositoryClient _dependenciesRepositoryClient;
        private readonly NetworkRunner _networkRunner;

        private bool IsServer => _networkRunner.IsServer;
        public bool IsInitialized => DependenciesRepository.IsInitialized;
        public IEnumerable<object> Instances => DependenciesRepository.Instances;
        
        private IAsyncDependenciesRepository DependenciesRepository
        {
            get
            {
                if (_dependenciesRepositoryHost != null && IsServer)
                    return _dependenciesRepositoryHost;
                if (_dependenciesRepositoryClient != null && IsServer == false)
                    return _dependenciesRepositoryClient;
                throw new Exception($"{nameof(GeneralAsyncDependenciesRepository)} not initialized!");
            }
        }

        public GeneralAsyncDependenciesRepository(
            [InjectOptional] AsyncDependenciesRepository dependenciesRepositoryHost, 
            [InjectOptional] AsyncDependenciesRepositoryClient dependenciesRepositoryClient,
            NetworkRunner networkRunner)
        {
            _dependenciesRepositoryHost = dependenciesRepositoryHost;
            _dependenciesRepositoryClient = dependenciesRepositoryClient;
            _networkRunner = networkRunner;
        }

        public UniTask InitializeAsync() => 
            DependenciesRepository.InitializeAsync();

        public UniTask<T> GetInstanceAsync<T>() where T : class => 
            DependenciesRepository.GetInstanceAsync<T>();
    }
}