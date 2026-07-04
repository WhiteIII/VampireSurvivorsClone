using Cysharp.Threading.Tasks;
using Fusion;

namespace _Project.Scripts.CompositionRoot.Services
{
    public abstract class InjectNetworkBehaviour : NetworkBehaviour
    {
        private bool _isSpawned = false;
        
        [Networked, UnityNonSerialized] public NetworkBool IsInitializeEnd { get; set; } = false;

        protected UniTask InitializeTask
        {
            get
            {
                if (IsInitializeEnd)
                    return UniTask.CompletedTask;
                return UniTask.WaitWhile(() => IsInitializeEnd == false);
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

        public override sealed async void Spawned()
        {
            _isSpawned = true;
            await UniTask.WaitWhile(() => IsInitializeEnd == false);
            OnSpawnMethod();
        }

        protected void EndInitialization() =>
            IsInitializeEnd = true;

        protected async UniTask<bool> GetStateAuthorityAsync()
        {
            await WaitToSpawnTask;
            return HasStateAuthority;
        }

        protected virtual void OnSpawnMethod() { }
    }
}