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
        [Networked, UnityNonSerialized] public int Health { get; set; }
        [Networked, UnityNonSerialized] public int Damage { get; set; }
        [Networked, UnityNonSerialized] public float AttackCooldown { get; set; }
        [Networked, UnityNonSerialized] public float MovementSpeed { get; set; }
        [Networked, UnityNonSerialized] public float AttackDistance { get; set; }

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
            
            Health = playerData.Health;
            Damage = playerData.Damage;
            AttackCooldown = playerData.AttackCooldown;
            MovementSpeed = playerData.MovementSpeed;
            AttackDistance = playerData.AttackDistance;
            EndInitialization();
        }

        public void Setup(Action<int> onSetHealth, Action<float> onSetMovementSpeed)
        {
            _onSetHealth = onSetHealth;
            _onSetMovementSpeed = onSetMovementSpeed;
        }

        public void SetHealth(int health)
        {
            Health = health;
            _onSetHealth.Invoke(health);
        }
        
        public void SetDamage(int damage) =>
            Damage = damage;
        
        public void SetAttackCoolDown(int coolDown) =>
            AttackCooldown = coolDown;

        public void SetMovementSpeed(float movementSpeed)
        {
            MovementSpeed = movementSpeed;
            _onSetMovementSpeed.Invoke(movementSpeed);
        }
    }
}