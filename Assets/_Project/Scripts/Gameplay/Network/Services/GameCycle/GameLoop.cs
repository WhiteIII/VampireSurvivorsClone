using System.Collections.Generic;
using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network.Services.HostMigration;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Fusion;

namespace _Project.Scripts.Gameplay.Network.Services.GameCycle
{
    public class GameLoop : NetworkBehaviour, IOnHostMigration
    {
        private readonly List<IUpdatable> _updatables = new();
        private readonly Queue<IUpdatable> _addedUpdateablesQueue = new();
        private readonly Queue<IUpdatable> _removedUpdateablesQueue = new();
        
        private bool _isPaused;

        public void OnHostMigration(GeneralNetworkObjectsRepository generalNetworkObjectsRepository)
        {
            List<NetworkObject> networkObjects = 
                generalNetworkObjectsRepository.CurrentNetworkRunner.GetAllNetworkObjects();

            foreach (NetworkObject networkObject in networkObjects)
            {
                foreach (IUpdatable updatable in networkObject.GetComponentsInChildren<IUpdatable>())
                    Register(updatable);
            }
        }

        public override void FixedUpdateNetwork()
        {
            if (HasStateAuthority == false)
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