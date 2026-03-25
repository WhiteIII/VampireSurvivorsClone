using System.Collections.Generic;
using Fusion;

namespace _Project.Scripts.Gameplay.Network.Services.GameCycle
{
    public class GameLoop : NetworkBehaviour
    {
        private readonly List<IUpdatable> _updatables = new();
        private readonly Queue<IUpdatable> _addedUpdateablesQueue = new();
        private readonly Queue<IUpdatable> _removedUpdateablesQueue = new();
        
        private bool _isPaused;

        public override void FixedUpdateNetwork()
        {
            foreach (IUpdatable updatable in _updatables)
                updatable.GameLoopUpdate();

            while (_addedUpdateablesQueue.Count > 0)
                _updatables.Add(_addedUpdateablesQueue.Dequeue());
            while (_removedUpdateablesQueue.Count > 0)
                _updatables.Remove(_removedUpdateablesQueue.Dequeue());
        }
        
        public T TryRegister<T>(T item)
        {
            if (item is IUpdatable gameLoopObject)
                Register(gameLoopObject);
            return item;
        } 
        
        public T Register<T>(T item) where T : IUpdatable
        {
            _addedUpdateablesQueue.Enqueue(item);
            return item;
        }

        public void TryUnregister<T>(T item)
        {
            if (item is IUpdatable updatable == false)
                return;
            if (_updatables.Contains(updatable))
                Unregister(updatable);
        }

        public void Unregister(IUpdatable updatable) => 
            _removedUpdateablesQueue.Enqueue(updatable);
    }
}