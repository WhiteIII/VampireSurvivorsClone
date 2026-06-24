using R3;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Network.Services.BaseComponent
{
    [RequireComponent(typeof(Health))]
    public abstract class DamageableCharacter : Character
    {
        public ReadOnlyReactiveProperty<int> OnHealthChanged { get; private set; }
        public ReadOnlyReactiveProperty<int> OnMaxHealthChanged { get; private set; }
        public Observable<Character> OnDead => _onDead;

        private readonly Subject<Character> _onDead = new();
        private Health _health;

        protected override void OnAwake()
        {
            _health = GetComponent<Health>();
            OnHealthChanged = _health.OnHealthChanged;
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