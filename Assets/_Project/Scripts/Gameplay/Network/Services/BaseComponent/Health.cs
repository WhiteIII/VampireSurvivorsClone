using Fusion;
using R3;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Network.Services.BaseComponent
{
    public class Health : NetworkBehaviour
    {
        public ReadOnlyReactiveProperty<int> OnHealthChanged { get; private set; }
        public ReadOnlyReactiveProperty<int> OnMaxHealthChanged { get; private set; }
        public Observable<Unit> OnDead => _onDead;
        public Observable<Unit> OnRevive => _onRevive;
        
        private readonly Subject<Unit> _onDead = new();
        private readonly Subject<Unit> _onRevive = new();
        
        [Networked] private int NetworkedMaxHealth { get; set; }
        [Networked] private int NetworkedCurrentHealth { get; set; }
        
        public override void Spawned()
        {
            OnHealthChanged = Observable
                .EveryValueChanged(this, x => x.NetworkedCurrentHealth)
                .ToReadOnlyReactiveProperty()
                .AddTo(this);
            OnMaxHealthChanged = Observable
                .EveryValueChanged(this, x => x.NetworkedMaxHealth)
                .ToReadOnlyReactiveProperty()
                .AddTo(this);
        }

        public void Initialize(int maxHealth)
        {
            NetworkedMaxHealth = maxHealth;
            NetworkedCurrentHealth = maxHealth;
        }

        public void SetMaxHealth(int maxHealth) => 
            NetworkedMaxHealth = maxHealth;

        public void Revive()
        {
            NetworkedCurrentHealth = NetworkedMaxHealth;
            RPC_OnRevive();
        } 

        public void TakeDamage(int damage)
        {
            NetworkedCurrentHealth = Mathf.Max(0, NetworkedCurrentHealth - damage);
            if (NetworkedCurrentHealth == 0)
                RPC_OnDead();
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsHostPlayer)]
        private void RPC_OnDead() => 
            _onDead.OnNext(Unit.Default);
        
        [Rpc(RpcSources.StateAuthority, RpcTargets.All, HostMode = RpcHostMode.SourceIsHostPlayer)]
        private void RPC_OnRevive() => 
            _onRevive.OnNext(Unit.Default);
    }
}