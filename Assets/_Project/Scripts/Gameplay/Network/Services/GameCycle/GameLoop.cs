using System.Collections.Generic;
using Fusion;

namespace _Project.Scripts.Gameplay.Network.Services.GameCycle
{
    public class GameLoop : NetworkBehaviour, IPausedAndResumeObject
    {
        private readonly List<IUpdatable> _updatables = new();
        private readonly List<IPausedCharacter> _pausedCharacters = new();
        private readonly Queue<IUpdatable> _addedUpdateablesQueue = new();
        private readonly Queue<IUpdatable> _removedUpdateablesQueue = new();
        
        private bool _isPaused;

        public override void FixedUpdateNetwork()
        {
            if (_isPaused)
                return;
            
            foreach (IUpdatable updatable in _updatables)
                updatable.GameLoopUpdate();

            while (_addedUpdateablesQueue.Count > 0)
                _updatables.Add(_addedUpdateablesQueue.Dequeue());
            while (_removedUpdateablesQueue.Count > 0)
                _updatables.Remove(_removedUpdateablesQueue.Dequeue());
        }
        
        public void AddUpdatable(IUpdatable updatable) =>
            _addedUpdateablesQueue.Enqueue(updatable);
        
        public void RemoveUpdatable(IUpdatable updatable) => 
            _removedUpdateablesQueue.Enqueue(updatable);
        
        public void AddPausedObject(IPausedCharacter pausedObject) => 
            _pausedCharacters.Add(pausedObject);
        
        public void RemovePausedObject(IPausedCharacter pausedObject) => 
            _pausedCharacters.Remove(pausedObject);

        public void Pause()
        {
            _isPaused = true;
            foreach (IPausedCharacter pausedObject in  _pausedCharacters)
                pausedObject.OnPause();
        }

        public void Resume() =>
            _isPaused = false;
    }
}