using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Project.Scripts.Common.AssetsManagement
{
    public class LocalAssetProvider
    {
        private readonly Dictionary<AssetReference, Object> _loadedAssets = new();

        public async UniTask LoadAsync(AssetReference assetReference)
        {
            if (_loadedAssets.ContainsKey(assetReference))
                return;
            Object asset = await Addressables.LoadAssetAsync<Object>(assetReference).Task;
            _loadedAssets.Add(assetReference, asset);
        }
        
        public T GetAsset<T>(AssetReference assetReference) where T : Object => _loadedAssets[assetReference] as T;

        public void ReleaseAsset(AssetReference assetReference)
        {
            Addressables.Release(_loadedAssets[assetReference]);
            _loadedAssets.Remove(assetReference);
        }
    }
}
