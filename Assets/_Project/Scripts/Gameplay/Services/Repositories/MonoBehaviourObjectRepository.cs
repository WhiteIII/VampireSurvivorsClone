using System.Collections.Generic;
using _Project.Scripts.Common.Repositories.Base;
using UnityEngine;

namespace _Project.Scripts.Gameplay.Services.Repositories
{
    public class MonoBehaviourObjectRepository : IRepository<MonoBehaviour>
    {
        private readonly List<MonoBehaviour> _objects = new();
        
        public T Add<T>(T monoBehaviour) where T : MonoBehaviour
        {
            _objects.Add(monoBehaviour);
            return monoBehaviour;
        }

        public bool TryGet<T>(out T result) where T : MonoBehaviour
        {
            result = null;
            if (_objects.Count == 0)
                return false;
            foreach (MonoBehaviour monoBehaviour in _objects)
            {
                if (monoBehaviour is T concreteObject)
                {
                    result = concreteObject;
                    return true;
                }
            }
            return false;
        }
        
        public void Remove(MonoBehaviour monoBehaviour) => 
            _objects.Remove(monoBehaviour);

        public void DestroyAllObjects()
        {
            foreach (MonoBehaviour monoBehaviour in _objects)
                Object.Destroy(monoBehaviour);
            _objects.Clear();
        }
    }
}
