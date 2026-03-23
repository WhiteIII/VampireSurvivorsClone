using System;

namespace _Project.Scripts.Gameplay.Network.Services.GameCycle
{
    public class GameLoopRegistrationHelper
    {
        private readonly GameLoop _gameLoop;

        public T TryRegister<T>(T item)
        {
            if (item is IGameLoopObject gameLoopObject)
                Register(gameLoopObject);
            return item;
        } 
        
        public T Register<T>(T item) where T : IGameLoopObject
        {
            if (item is IUpdatable updatable)
            {
                _gameLoop.AddUpdatable(updatable);
                return item;
            }
            if (item is IPausedCharacter pausedObject)
            {
                _gameLoop.AddPausedObject(pausedObject);
                return item;
            }

            throw new Exception("Cannot register a game loop object!");
        }

        public void Unregister(IGameLoopObject gameLoopObject)
        {
            if (gameLoopObject is IUpdatable updatable)
                _gameLoop.RemoveUpdatable(updatable);
            else if (gameLoopObject is IPausedCharacter pausedObject)
                _gameLoop.RemovePausedObject(pausedObject);
        }
    }
}