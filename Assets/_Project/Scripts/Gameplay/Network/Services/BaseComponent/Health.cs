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
        
        private readonly ReactiveProperty<int> _onHealthChanged = new();
        private readonly ReactiveProperty<int> _onMaxHealthChanged = new();
        private readonly Subject<Unit> _onDead = new();

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

        public void SetMaxHealth(int maxHealth)
        {
            int pastHealth = NetworkedMaxHealth;
            NetworkedMaxHealth = maxHealth;
            _onMaxHealthChanged.OnNext(NetworkedMaxHealth);
        } 

        public void Revive() => 
            NetworkedCurrentHealth = NetworkedMaxHealth;

        public void TakeDamage(int damage)
        {
            NetworkedCurrentHealth = Mathf.Max(0, NetworkedCurrentHealth - damage);
            _onHealthChanged.OnNext(NetworkedCurrentHealth);
            if (NetworkedCurrentHealth == 0)
                _onDead.OnNext(Unit.Default);
        }
    }
}