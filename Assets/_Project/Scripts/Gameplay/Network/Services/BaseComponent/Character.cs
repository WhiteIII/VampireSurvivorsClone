using Fusion;
using R3;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Network.Services.BaseComponent
{
    public class Character : NetworkBehaviour
    {
        public ReadOnlyReactiveProperty<Vector3> Position { get; private set; }
        [Networked] private NetworkBool NetworkIsActive { get; set; }
        
        public bool IsActive => NetworkIsActive;

        public sealed override void Spawned() => 
            OnSpawned();

        private void Awake()
        {
            Position = Observable
                .EveryValueChanged(transform, x => x.position)
                .ToReadOnlyReactiveProperty()
                .AddTo(this);
            OnAwake();
        }

        public void Enable() => 
            NetworkIsActive = true;

        public void Disable() => 
            NetworkIsActive = false;
        
        protected virtual void OnAwake() { }
        
        protected virtual void OnSpawned() { }
    }
}