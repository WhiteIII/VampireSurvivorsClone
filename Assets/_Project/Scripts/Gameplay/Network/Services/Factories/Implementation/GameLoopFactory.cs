using _Project.Scripts.Gameplay.Network.Services.Factories.Base;
using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Implementation;
using _Project.Scripts.Gameplay.Network.Services.GameCycle;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Implementation
{
    public class GameLoopFactory : NetworkObjectFactory<GameLoop>
    {
        public GameLoopFactory(
            [Inject(Id = "GameLoopAssetReference")]AssetReference prefabAssetReference, 
            NetworkObjectsCreator creator) : base(prefabAssetReference, creator)
        {
        }
    }
}