using UnityEngine;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.BaseComponent.Local
{
    public class CameraSetter : MonoBehaviour
    {
        [SerializeField] private Transform _cameraAnchorPoint;
        
        private CameraController _cameraController;
        
        [Inject] private void Construct(CameraController cameraController) => 
            _cameraController = cameraController;
        //TODO Construct вызывается сразу у двух игроков, от сюда баги с камерой
        
        private void LateUpdate()
        {
            _cameraController.MoveTo(_cameraAnchorPoint.position);
        }
    }
}