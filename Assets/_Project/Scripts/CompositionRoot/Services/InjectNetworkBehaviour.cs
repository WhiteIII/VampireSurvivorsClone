using Cysharp.Threading.Tasks;
using Fusion;

namespace _Project.Scripts.CompositionRoot.Services
{
    public abstract class InjectNetworkBehaviour : NetworkBehaviour
    {
        private bool _isSpawned;
        
        [Networked, UnityNonSerialized] private NetworkBool IsInitializeEndNetwork { get; set; } = false;

        public bool IsInitializeEnd => IsInitializeEndNetwork;
        
        public UniTask InitializeTask
        {
            get
            {
                if (IsInitializeEndNetwork)
                    return UniTask.CompletedTask;
                return UniTask.WaitWhile(() => IsInitializeEndNetwork == false);
            }
        }

        private UniTask WaitToSpawnTask
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
            await UniTask.WaitWhile(() => IsInitializeEndNetwork == false);
            OnSpawnMethod();
        }

        protected void EndInitialization() =>
            IsInitializeEndNetwork = true;

        protected async UniTask<bool> GetStateAuthorityAsync()
        {
            await WaitToSpawnTask;
            return HasStateAuthority;
        }

        protected virtual void OnSpawnMethod() { }
    }
}