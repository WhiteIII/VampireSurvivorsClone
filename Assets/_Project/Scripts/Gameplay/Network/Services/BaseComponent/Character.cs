using Fusion;
using R3;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Network.Services.BaseComponent
{
    public class Character : NetworkBehaviour
    {
        public ReadOnlyReactiveProperty<Vector3> Position { get; private set; }
        [Networked] public uint Id { get; set; }

        public void SetId(uint id) => 
            Id = id;
        
        private void Awake()
        {
            Position = Observable
                .EveryValueChanged(transform, x => x.position)
                .ToReadOnlyReactiveProperty()
                .AddTo(this);
        }
    }
}