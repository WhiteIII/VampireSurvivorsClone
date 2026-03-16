using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Common.AssetsManagement
{
    public class AssetsLoader
    {
        private readonly LocalAssetProvider _localAssetProvider;
        private readonly List<AssetReference> _notLoadedAssets = new();
        private readonly List<AssetReference> _loadedAssets = new();
        
        public AssetsLoader(
            LocalAssetProvider localAssetProvider,
            AssetReference[] loadedAssets = null)
        {
            _localAssetProvider = localAssetProvider;
            if (loadedAssets != null)
                _notLoadedAssets.AddRange(loadedAssets);
        }

        public UniTask[] GetLoadedTaskAssets()
        {
            List<UniTask> tasks = new(_notLoadedAssets.Count);
            foreach (AssetReference assetReference in _notLoadedAssets)
            {
                tasks.Add(_localAssetProvider.LoadAsync(assetReference));
                _loadedAssets.Add(assetReference);
            }
            _notLoadedAssets.Clear();
            return tasks.ToArray();
        }
        
        public void AddAsset(AssetReference assetReference) => 
            _notLoadedAssets.Add(assetReference);

        public void UnloadAssets(params AssetReference[] assets)
        {
            foreach (AssetReference assetReference in assets)
            {
                if (_loadedAssets.Contains(assetReference))
                {
                    _localAssetProvider.ReleaseAsset(assetReference);
                    _loadedAssets.Remove(assetReference);
                }
            }
        }
    }
}