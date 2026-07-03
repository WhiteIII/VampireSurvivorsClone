using System;
using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Configs.Base;
using _Project.Scripts.Configs.Services.Base;
using Fusion;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.BaseComponent.UpgradeSystem
{
    public class PlayerRunTimeDataNetwork : InjectNetworkBehaviour, IPlayerData
    {
        [Networked, UnityNonSerialized] private int NetworkedHealth { get; set; }
        [Networked, UnityNonSerialized] private int NetworkedDamage { get; set; }
        [Networked, UnityNonSerialized] private float NetworkedAttackCooldown { get; set; }
        [Networked, UnityNonSerialized] private float NetworkedMovementSpeed { get; set; }
        [Networked, UnityNonSerialized] private float NetworkedAttackDistance { get; set; }

        public int Health => NetworkedHealth;
        public int Damage => NetworkedDamage;
        public float AttackCooldown => NetworkedAttackCooldown;
        public float MovementSpeed => NetworkedMovementSpeed;
        public float AttackDistance => NetworkedAttackDistance;
        
        private Action<int> _onSetHealth;
        private Action<float> _onSetMovementSpeed;

        [Inject] private async void Construct(IConfigService configService)
        {
            bool hasStateAuthority = await GetStateAuthorityAsync();
            if (hasStateAuthority == false)
            {
                EndInitialization();
                return;
            }
            
            IPlayerData playerData = configService.GetConfig<IPlayerData>();
            
            NetworkedHealth = playerData.Health;
            NetworkedDamage = playerData.Damage;
            NetworkedAttackCooldown = playerData.AttackCooldown;
            NetworkedMovementSpeed = playerData.MovementSpeed;
            NetworkedAttackDistance = playerData.AttackDistance;
            EndInitialization();
        }

        public void Setup(Action<int> onSetHealth, Action<float> onSetMovementSpeed)
        {
            _onSetHealth = onSetHealth;
            _onSetMovementSpeed = onSetMovementSpeed;
        }

        public void SetHealth(int health)
        {
            NetworkedHealth = health;
            _onSetHealth.Invoke(health);
        }

        public void SetDamage(int damage) =>
            NetworkedDamage = damage;

        public void SetAttackCoolDown(int coolDown) =>
            NetworkedAttackCooldown = coolDown;

        public void SetMovementSpeed(float movementSpeed)
        {
            NetworkedMovementSpeed = movementSpeed;
            _onSetMovementSpeed.Invoke(movementSpeed);
        }
    }
}