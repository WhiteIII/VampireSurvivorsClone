using _Project.Scripts.Gameplay.Network.Services.BaseComponent.UpgradeSystem;
using _Project.Scripts.Gameplay.Network.Services.GameCycle;
using _Project.Scripts.Gameplay.Network.Services.InputSystem;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Network.Services.BaseComponent
{
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(AttackSystem))]
    [RequireComponent(typeof(PlayerRunTimeDataNetwork))]
    public class Player : NetworkBehaviour, IUpdatable
    {
        private PlayerMovement _playerMovement;
        private AttackSystem _attackSystem;
        
        public PlayerRef PlayerRef { get; private set; }
        
        public void Initialize(PlayerRef playerRef) =>  
            PlayerRef = playerRef;

        public override void Spawned()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _attackSystem = GetComponent<AttackSystem>();
        }
        
        public void SetPosition(Vector3 position) =>
            _playerMovement.SetPosition(position);

        public void GameLoopUpdate()
        {
            if (GetInput(out InputData inputData))
                _playerMovement.Move(inputData.Direction.normalized);
            _attackSystem.TryAttack();
        }
    }
}