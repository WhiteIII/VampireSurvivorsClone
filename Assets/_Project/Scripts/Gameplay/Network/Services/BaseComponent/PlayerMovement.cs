using Fusion;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Network.Services.BaseComponent
{
    [RequireComponent(typeof(NetworkCharacterController))]
    public class PlayerMovement : NetworkBehaviour
    {
        private NetworkCharacterController _characterController;
        
        private void Awake() => 
            _characterController = GetComponent<NetworkCharacterController>();
        
        public void Move(Vector3 direction) =>
            _characterController.Move(direction * Runner.DeltaTime * 5);
        
        public void SetPosition(Vector3 position) =>
            _characterController.transform.position = position;
    }
}