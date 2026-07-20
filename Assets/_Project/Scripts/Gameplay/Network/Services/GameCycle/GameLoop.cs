using System.Collections.Generic;
using _Project.Scripts.CompositionRoot.Services;
using Cysharp.Threading.Tasks;
using Fusion;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.GameCycle
{
    public class GameLoop : NetworkBehaviour, IAfterHostMigration
    {
        private readonly List<IUpdatable> _updatables = new();
        private readonly Queue<IUpdatable> _addedUpdateablesQueue = new();
        private readonly Queue<IUpdatable> _removedUpdateablesQueue = new();

        private bool _isActive;

        [Inject] private async UniTask Construct(GameLoopLocalBuffer buffer)
        {
            await buffer.InitializeAsync();
            foreach (IUpdatable updatable in buffer.Updatables)
                Register(updatable);
        }
        
        public void AfterHostMigration()
        {
            List<NetworkObject> networkObjects = Runner.GetAllNetworkObjects();

            foreach (NetworkObject networkObject in networkObjects)
            {
                foreach (IUpdatable updatable in networkObject.GetComponentsInChildren<IUpdatable>())
                    Register(updatable);
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (HasStateAuthority == false || _isActive == false)
                return;

            foreach (IUpdatable updatable in _updatables)
            {
                if (updatable is InjectNetworkBehaviour injectNetworkBehaviour)
                {
                    if (injectNetworkBehaviour.IsInitializeEnd == false)
                        continue;
                }
                updatable.GameLoopUpdate();
            }

            while (_addedUpdateablesQueue.Count > 0)
                _updatables.Add(_addedUpdateablesQueue.Dequeue());
            while (_removedUpdateablesQueue.Count > 0)
                _updatables.Remove(_removedUpdateablesQueue.Dequeue());
        }

        public void Enable()
        {
            if (HasStateAuthority)
                _isActive = true;
        }

        public void Disable()
        {
            if (HasStateAuthority)
                _isActive = false;
        }
        
        public T TryRegister<T>(T item) 
            where T : NetworkBehaviour
        {
            if (item is IUpdatable gameLoopObject)
                Register(gameLoopObject);
            return item;
        }

        private void Register(IUpdatable item) =>       
            _addedUpdateablesQueue.Enqueue(item);

        public void TryUnregister<T>(T item) 
            where T : NetworkBehaviour
        {
            if (item is IUpdatable updatable == false)
                return;
            if (_updatables.Contains(updatable))
                Unregister(updatable);
        }

        private void Unregister(IUpdatable updatable) => 
            _removedUpdateablesQueue.Enqueue(updatable);
    }
}