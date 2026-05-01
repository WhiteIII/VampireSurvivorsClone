using Cysharp.Threading.Tasks;
using Fusion;

namespace _Project.Scripts.CompositionRoot.Services
{
    public abstract class InjectNetworkBehaviour : NetworkBehaviour
    {
        private bool IsReady = false;

        protected UniTask InitializeTask
        {
            get
            {
                if (IsReady)
                    return UniTask.CompletedTask;
                return UniTask.WaitWhile(() => IsReady == false);
            }
        }
        
        protected void EndInitialization() =>
            IsReady = true;

        public override async void Spawned()
        {
            await UniTask.WaitWhile(() => IsReady == false);
            OnSpawn();
        }

        protected virtual void OnSpawn() { }
    }
}