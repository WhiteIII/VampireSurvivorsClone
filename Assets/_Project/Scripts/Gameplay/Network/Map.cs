using UnityEngine;

namespace _Project.Scripts.Gameplay.Network
{
    public class Map : MonoBehaviour
    {
        public Vector2 MapSize { get; private set; }

        private void Awake()
        {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            Vector3 mashSize = meshRenderer.bounds.size;
            MapSize = new Vector2(mashSize.x, mashSize.z);
        }
    }
}