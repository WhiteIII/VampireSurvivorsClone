using R3;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Network.Services.BaseComponent
{
    [RequireComponent(typeof(Health))]
    public abstract class DamageableCharacter : Character
    {
        public ReadOnlyReactiveProperty<int> OnHealthChanged { get; private set; }
        public ReadOnlyReactiveProperty<int> OnMaxHealthChanged { get; private set; }
        public Observable<DamageableCharacter> OnDead => _onDead;
        public Observable<DamageableCharacter> OnRevive => _onRevive;
        
        private readonly Subject<DamageableCharacter> _onDead = new();
        private readonly Subject<DamageableCharacter> _onRevive = new();
            
        private Health _health;

        protected override void OnAwake()
        {
            _health = GetComponent<Health>();
            OnHealthChanged = _health.OnHealthChanged;
            OnMaxHealthChanged = _health.OnMaxHealthChanged;
            _health
                .OnDead
                .Subscribe(_ => _onDead.OnNext(this))
                .AddTo(this);
        }

        public void TakeDamage(int damage) => 
            _health.TakeDamage(damage);

        public void Revive()
        {
            
        }
    }
}