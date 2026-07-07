using UnityEngine;

namespace _Project.Scripts.Gameplay.Network.Services.BaseComponent.Local
{
    public class CameraController
    {
        private readonly Camera _camera;

        public CameraController(Camera camera) => 
            _camera = camera;
        
        public void MoveTo(Vector3 position) => 
            _camera.transform.position = position;
    }
}