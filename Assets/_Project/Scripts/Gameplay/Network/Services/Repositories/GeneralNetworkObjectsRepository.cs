using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Common.Services.Repositories.Base;
using _Project.Scripts.Gameplay.Network.Services.Factories.NetworkObjectProvider;
using Fusion;
using Behaviour = Fusion.Behaviour;
using Object = UnityEngine.Object;

namespace _Project.Scripts.Gameplay.Network.Services.Repositories
{
    public partial class GeneralNetworkObjectsRepository : IRepository<Behaviour>
    {
        public NetworkRunner CurrentNetworkRunner { get; private set; }
        public NetworkSceneManagerDefault CurrentNetworkSceneManager { get; private set; }
        public NetworkObjectEndEmptyObjectProvider CurrentNetworkObjectProvider { get; private set; }
        
        public int Count
        {
            get
            {
                int count = 0;
                if (CurrentNetworkRunner)
                    count++;
                if (CurrentNetworkSceneManager)
                    count++;
                if (CurrentNetworkObjectProvider)
                    count++;
                return count;
            }
        }

        public T Add<T>(T item) where T : Behaviour
        {
            if (CurrentNetworkRunner && CurrentNetworkSceneManager && CurrentNetworkObjectProvider)
                throw new Exception("All the objects are already installed!");
            if (item is NetworkRunner networkRunner)
            {
                if (CurrentNetworkRunner)
                    throw new Exception("Only one NetworkRunner is allowed!");
                CurrentNetworkRunner = networkRunner;
                return item;
            }

            if (item is NetworkSceneManagerDefault networkSceneManager)
            {
                if (CurrentNetworkSceneManager)
                    throw new Exception("Only one NetworkSceneManagerDefault is allowed!");
                CurrentNetworkSceneManager = networkSceneManager;
                return item;
            }

            if (item is NetworkObjectEndEmptyObjectProvider networkObjectProvider)
            {
                if (CurrentNetworkObjectProvider)
                    throw new Exception("Only one NetworkObjectProvider is allowed!");
                CurrentNetworkObjectProvider = networkObjectProvider;
                return item;
            }

            throw new Exception("The object is not defined!");
        }

        public bool TryGet<T>(out T item) where T : Behaviour
        {
            item = null;
            if (typeof(T) == typeof(NetworkSceneManagerDefault))
            {
                if (CurrentNetworkSceneManager)
                {
                    item = CurrentNetworkSceneManager as T;
                    return true;
                }
            }
            else if (typeof(T) == typeof(NetworkRunner))
            {
                if (CurrentNetworkRunner)
                {
                    item = CurrentNetworkRunner as T;
                    return true;
                }
            }
            else if (typeof(T) == typeof(NetworkObjectEndEmptyObjectProvider))
            {
                if (CurrentNetworkObjectProvider)
                {
                    item = CurrentNetworkObjectProvider as T;
                    return true;
                }
            }

            return false;
        }

        public void Remove(Behaviour item)
        {
            if (ReferenceEquals(item, CurrentNetworkRunner))
                CurrentNetworkRunner = null;
            else if (ReferenceEquals(item, CurrentNetworkSceneManager))
                CurrentNetworkSceneManager = null;
            else if (ReferenceEquals(item, CurrentNetworkObjectProvider))
                CurrentNetworkObjectProvider = null;
        }

        public void DestroyNetworkRunnerAndSceneManager()
        {
            Object.Destroy(CurrentNetworkRunner);
            Object.Destroy(CurrentNetworkSceneManager);
            Object.Destroy(CurrentNetworkObjectProvider);
            CurrentNetworkRunner = null;
            CurrentNetworkSceneManager = null;
            CurrentNetworkObjectProvider = null;
        }

        public IEnumerator<Behaviour> GetEnumerator() =>
            new GeneralNetworkObjectsEnumerator(
                CurrentNetworkRunner, 
                CurrentNetworkSceneManager, 
                CurrentNetworkObjectProvider);

        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }

    public partial class GeneralNetworkObjectsRepository
    {
        private struct GeneralNetworkObjectsEnumerator : IEnumerator<Behaviour>
        {
            private readonly NetworkRunner _networkRunner;
            private readonly NetworkSceneManagerDefault _networkSceneManager;
            private readonly NetworkObjectProviderDefault _networkObjectProvider;
            private Behaviour _current;
            private bool _networkRunnerIsCheck;
            private bool _networkSceneManagerIsCheck;
            private bool _networkObjectProviderIsCheck;

            public Behaviour Current => _current;
            object IEnumerator.Current => Current;

            public GeneralNetworkObjectsEnumerator(
                NetworkRunner networkRunner,
                NetworkSceneManagerDefault networkSceneManager, 
                NetworkObjectProviderDefault networkObjectProvider) : this()
            {
                _networkRunner = networkRunner;
                _networkSceneManager = networkSceneManager;
                _networkObjectProvider = networkObjectProvider;
                _networkRunnerIsCheck = false;
                _networkSceneManagerIsCheck = false;
                _networkObjectProviderIsCheck = false;
            }

            public bool MoveNext()
            {
                if (_networkRunnerIsCheck == false)
                {
                    _networkRunnerIsCheck = true;
                    if (_networkRunner)
                    {
                        _current = _networkRunner;
                        return true;
                    }
                }
                if (_networkSceneManagerIsCheck == false)
                {
                    _networkSceneManagerIsCheck = true;
                    if (_networkSceneManager)
                    {
                        _current = _networkSceneManager;
                        return true;
                    }
                }
                if (_networkObjectProviderIsCheck == false)
                {
                    _networkObjectProviderIsCheck = true;
                    if (_networkObjectProvider)
                    {
                        _current = _networkObjectProvider;
                        return true;
                    }
                }

                return false;
            }

            public void Reset()
            {
                _networkRunnerIsCheck = false;
                _networkSceneManagerIsCheck = false;
                _networkObjectProviderIsCheck = false;
                _current = null;
            }

            public void Dispose() { }
        }
    }
}