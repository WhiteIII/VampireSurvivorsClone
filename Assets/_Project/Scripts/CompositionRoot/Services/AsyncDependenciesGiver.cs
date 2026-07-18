using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.CompositionRoot.Services
{
    public class AsyncDependenciesGiver
    {
        private readonly HashSet<IAsyncDependenceProvider<object>> _providers = new();
        private readonly HashSet<object> _instances = new();

        public void Unregister<T>()
        {
            if (TryGetCreatedInstance(out T instance))
                _instances.Remove(instance);
        }

        public void AddFactory<T>(IAsyncDependenceProvider<T> provider)
        {
            if (TryGetCreatedInstance(out T _))
                throw new Exception($"Type: {typeof(T).Name} already created!");
            if(TryGetProvider(out IAsyncDependenceProvider<T> _))
                throw new Exception($"Type: {typeof(IAsyncDependenceProvider<T>).Name} already added to container!");
            _providers.Add((IAsyncDependenceProvider<object>)provider);
        }

        public async UniTask<T> GetInstanceAsync<T>()
        {
            if (TryGetCreatedInstance(out T result))
                return result;
            if (TryGetProvider(out IAsyncDependenceProvider<T> provider))
            {
                await provider.CreateAsync();
                _providers.Remove((IAsyncDependenceProvider<object>)provider);
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

        private bool TryGet<T>(out T item, IEnumerable<T> list, Predicate<T> predicate)
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