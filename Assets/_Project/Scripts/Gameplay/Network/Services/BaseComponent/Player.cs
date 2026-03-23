using _Project.Scripts.Gameplay.Network.Services.GameCycle;
using _Project.Scripts.Gameplay.Network.Services.InputSystem;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Network.Services.BaseComponent
{
    [RequireComponent(typeof(PlayerMovement))]
    public class Player : NetworkBehaviour, IUpdatable
    {
        private PlayerMovement _playerMovement;
        
        public PlayerRef PlayerRef { get; private set; }
        
        public void Initialize(PlayerRef playerRef) =>  
            PlayerRef = playerRef;

        private void Awake() => 
            _playerMovement = GetComponent<PlayerMovement>();
        
        public void SetPosition(Vector3 position) =>
            _playerMovement.SetPosition(position);

        public void GameLoopUpdate()
        {
            if (GetInput(out InputData inputData))
                _playerMovement.Move(inputData.Direction.normalized);
        }
    }
}