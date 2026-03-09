using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Services.Repositories
{
    public class MonoBehaviourObjectsRepository
    {
        private readonly List<MonoBehaviour> _objects = new();
        
        public T Add<T>(T monoBehaviour) where T : MonoBehaviour
        {
            _objects.Add(monoBehaviour);
            return monoBehaviour;
        }

        public void DestroyAllObjects()
        {
            foreach (MonoBehaviour monoBehaviour in _objects)
                Object.Destroy(monoBehaviour);
            _objects.Clear();
        }
    }
}
