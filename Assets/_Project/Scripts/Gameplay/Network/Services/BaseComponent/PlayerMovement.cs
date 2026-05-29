using _Project.Scripts.Gameplay.Network.Services.BaseComponent.UpgradeSystem;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Network.Services.BaseComponent
{
    [RequireComponent(typeof(NetworkCharacterController))]
    public class PlayerMovement : NetworkBehaviour
    {
        [Networked] private NetworkCharacterController CharacterController { get; set; }
        [Networked] private float MovementSpeed { get; set; }

        public override void Spawned() => 
            CharacterController = GetComponent<NetworkCharacterController>();

        public void SetMovementSpeed(float movementSpeed) => 
            MovementSpeed = movementSpeed;
        
        public void Move(Vector3 direction) =>
            CharacterController.Move(direction * Runner.DeltaTime * MovementSpeed);
        
        public void SetPosition(Vector3 position) =>
            CharacterController.transform.position = position;
    }
}