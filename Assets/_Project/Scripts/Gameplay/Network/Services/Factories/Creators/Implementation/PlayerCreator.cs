using _Project.Scripts.Common.AssetsManagement;
using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Base;
using _Project.Scripts.Gameplay.Network.Services.GameCycle;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Cysharp.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Implementation
{
    public class PlayerCreator : BaseNetworkObjectsCreator<Player>
    {
        [Inject] private void Construct(
            LocalAssetProvider localAssetProvider,
            GeneralNetworkObjectsRepository networkRunner, 
            PlayerRepository repository,
            GameLoop gameLoop)  
        {
            Initialize(localAssetProvider, networkRunner, repository, gameLoop);
        }
        
        public UniTask<T> Create<T>(AssetReference assetReference, Vector3 position, PlayerRef playerRef) where T : Player => 
            CreateWithParameters<T>(assetReference, position, null, playerRef);
    }
}