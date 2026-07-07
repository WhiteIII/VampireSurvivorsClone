using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.BaseComponent.Local
{
    public class CameraSetter : NetworkBehaviour
    {
        [SerializeField] private Transform _cameraAnchorPoint;
        
        private CameraController _cameraController;
        
        [Inject] private void Construct(CameraController cameraController) => 
            _cameraController = cameraController;
        
        private void LateUpdate()
        {
            if (HasInputAuthority == false)
                return;
            
            _cameraController.MoveTo(_cameraAnchorPoint.position);
        }
    }
}