using _Project.Scripts.Gameplay.Network.Services.BaseComponent.UpgradeSystem;
using Fusion;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Network.Services.BaseComponent
{
    [RequireComponent(typeof(NetworkCharacterController))]
    public class PlayerMovement : NetworkBehaviour
    {
        [Networked] private NetworkCharacterController CharacterController { get; set; }
        [Networked] private PlayerRunTimeDataNetwork PlayerData { get; set; }

        public override void Spawned()
        {
            CharacterController = GetComponent<NetworkCharacterController>();
            PlayerData = GetComponent<PlayerRunTimeDataNetwork>();
        }
        
        public void Move(Vector3 direction) =>
            CharacterController.Move(direction * Runner.DeltaTime * 5);
        
        public void SetPosition(Vector3 position) =>
            CharacterController.transform.position = position;
    }
}