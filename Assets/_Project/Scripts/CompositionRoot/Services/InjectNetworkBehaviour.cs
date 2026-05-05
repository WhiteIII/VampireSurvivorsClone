using Cysharp.Threading.Tasks;
using Fusion;

namespace _Project.Scripts.CompositionRoot.Services
{
    public abstract class InjectNetworkBehaviour : NetworkBehaviour
    {
        private bool _isInitializeEnd = false;
        private bool _isSpawned = false;
        
        protected UniTask InitializeTask
        {
            get
            {
                if (_isInitializeEnd)
                    return UniTask.CompletedTask;
                return UniTask.WaitWhile(() => _isInitializeEnd == false);
            }
        }

        protected UniTask WaitToSpawnTask
        {
            get
            {
                if (_isSpawned)
                    return UniTask.CompletedTask;
                return UniTask.WaitWhile(() => _isSpawned == false);
            }
        }

        public override async void Spawned()
        {
            _isSpawned = true;
            await UniTask.WaitWhile(() => _isInitializeEnd == false);
            OnSpawn();
        }

        protected void EndInitialization() =>
            _isInitializeEnd = true;

        protected async UniTask<bool> GetStateAuthorityAsync()
        {
            await WaitToSpawnTask;
            return HasStateAuthority;
        }

        protected virtual void OnSpawn() { }
    }
}