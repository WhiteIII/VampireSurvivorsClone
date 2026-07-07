using _Project.Scripts.Gameplay.Network.Services.BaseComponent.Local;
using _Project.Scripts.Gameplay.Network.Services.BaseComponent.UpgradeSystem;
using _Project.Scripts.Gameplay.Network.Services.GameCycle;
using _Project.Scripts.Gameplay.Network.Services.InputSystem;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Network.Services.BaseComponent
{
    [RequireComponent(typeof(CameraSetter))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(AttackSystem))]
    [RequireComponent(typeof(PlayerRunTimeDataNetwork))]
    [RequireComponent(typeof(Health))]
    public class Player : DamageableCharacter, IUpdatable
    {
        private PlayerMovement _playerMovement;
        private AttackSystem _attackSystem;
        private Health _health;
        
        [Networked] private PlayerRef NetworkedPlayerRef { get; set; }
        
        public PlayerRef PlayerRef => NetworkedPlayerRef;
        
        public void Initialize(PlayerRef playerRef) =>  
            NetworkedPlayerRef = playerRef;

        public override async void Spawned()
        {
            _playerMovement = GetComponent<PlayerMovement>();
            _attackSystem = GetComponent<AttackSystem>();
            _health = GetComponent<Health>();
            PlayerRunTimeDataNetwork playerData = GetComponent<PlayerRunTimeDataNetwork>();
            await UniTask.WaitWhile(() => playerData.IsInitializeEnd == false);
            playerData.Setup(
                x => _health.SetMaxHealth(x), 
                x => _playerMovement.SetMovementSpeed(x));
            _health.SetMaxHealth(playerData.Health);
            _playerMovement.SetMovementSpeed(playerData.MovementSpeed);
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