using _Project.Scripts.CompositionRoot.Services;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services
{
    public class EnemySpawnPositionHelper : InjectNetworkBehaviour
    {
        private Map _map;
        
        private Vector2 MapSize => _map.MapSize;
        
        [Inject] private async UniTask Construct(Map map)
        {
            await GetStateAuthorityAsync();
            _map = map;
            EndInitialization();
        }

        public Vector3 GetSpawnPosition() => 
            new(Random.Range(-MapSize.x/2f, MapSize.x/2f), 0, Random.Range(-MapSize.y/2f, MapSize.y/2f));
    }
}