using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.CompositionRoot.Services
{
    public class AsyncDependenciesGiver
    {
        private readonly List<IAsyncDependenceProvider<object>> _providers = new();
        private readonly List<object> _instances = new();
        
        public void AddFactory(IAsyncDependenceProvider<object> provider) => 
            _providers.Add(provider);

        public async UniTask<T> GetInstanceAsync<T>()
        {
            if (TryGetCreatedInstance(out T result))
                return result;
            if (TryGetProvider(out IAsyncDependenceProvider<T> provider))
            {
                await provider.CreateAsync();
                _instances.Add(provider.Value);
                return provider.Value;
            }
            throw new Exception($"{nameof(T)} Not Found!");
        }

        public bool Contains<T>()
        {
            if (TryGetCreatedInstance(out T _))
                return true;
            if (TryGetProvider(out IAsyncDependenceProvider<T> _))
                return true;
            return false;
        }

        private bool TryGetProvider<T>(out IAsyncDependenceProvider<T> provider)
        {
            provider = null;
            if (TryGet(out IAsyncDependenceProvider<object> foundProvider, _providers,
                    x => x is IAsyncDependenceProvider<T>))
            {
                provider = (IAsyncDependenceProvider<T>)foundProvider;
                return true;
            }
            return false;
        }

        private bool TryGetCreatedInstance<T>(out T instance)
        {
            instance = default;
            if (TryGet(out object createdInstance, _instances, x => x is T))
            {
                instance = (T)createdInstance;
                return true;
            }
            return false;
        }
        
        private bool TryGet<T>(out T item, List<T> list, Predicate<T> predicate)
        {
            item = default;
            foreach (T listItem in list)
            {
                if (predicate(listItem))
                {
                    item = listItem;
                    return true;
                }
            }
            return false;
        }
    }
}