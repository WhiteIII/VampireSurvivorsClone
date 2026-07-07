using _Project.Scripts.CompositionRoot.Services;
using _Project.Scripts.Gameplay.Network.Services.BaseComponent.UpgradeSystem;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.BaseComponent
{
    public class AttackSystem : InjectNetworkBehaviour
    {
        [Networked] private EnemyRepository Repository { get; set; }
        [Networked] private PlayerRunTimeDataNetwork PlayerData { get; set; }
        [Networked] private TickTimer Timer { get; set; }

        [Inject] private async UniTask Construct(AsyncDependenciesRepository asyncDependenciesRepository)
        {
            bool hasStateAuthority = await GetStateAuthorityAsync();
            if (hasStateAuthority == false)
            {
                EndInitialization();
                return;
            }
            Repository = await asyncDependenciesRepository.GetInstanceAsync<EnemyRepository>();
            EndInitialization();
        }

        protected override void OnSpawnMethod() => 
            PlayerData = GetComponent<PlayerRunTimeDataNetwork>();

        public void TryAttack()
        {
            if (HasStateAuthority == false)
                return;
            
            if (Timer.Expired(Runner))
            {
                Attack();
                Timer = TickTimer.CreateFromSeconds(Runner, PlayerData.AttackCooldown);   
            }
        }

        private void Attack()
        {
            foreach (Enemy enemy in Repository)
            {
                if (enemy is DamageableCharacter damageable && IsAttacked(enemy))
                    damageable.TakeDamage(PlayerData.Damage);
            }
        }

        private bool IsAttacked(Enemy enemy)
        {
            if (Vector3.Distance(transform.position, enemy.Position.CurrentValue) < PlayerData.AttackDistance)
                return true;
            return false;
        }
    }
}