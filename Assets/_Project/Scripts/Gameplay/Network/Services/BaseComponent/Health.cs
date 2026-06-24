using Fusion;
using R3;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Network.Services.BaseComponent
{
    public class Health : NetworkBehaviour
    {
        public ReadOnlyReactiveProperty<int> OnHealthChanged => _onHealthChanged;
        public ReadOnlyReactiveProperty<int> OnMaxHealthChanged => _onMaxHealthChanged;
        public Observable<Unit> OnDead => _onDead;
        
        private readonly ReactiveProperty<int> _onHealthChanged = new();
        private readonly ReactiveProperty<int> _onMaxHealthChanged = new();
        private readonly Subject<Unit> _onDead = new();

        [Networked] private int MaxHealth { get; set; }
        [Networked] private int CurrentHealth { get; set; }

        public void Initialize(int maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }

        public void SetMaxHealth(int maxHealth)
        {
            int pastHealth = MaxHealth;
            MaxHealth = maxHealth;
            _onMaxHealthChanged.OnNext(MaxHealth);
        } 

        public void Revive() => 
            CurrentHealth = MaxHealth;

        public void TakeDamage(int damage)
        {
            CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
            _onHealthChanged.OnNext(CurrentHealth);
            if (CurrentHealth == 0)
                _onDead.OnNext(Unit.Default);
        }
    }
}