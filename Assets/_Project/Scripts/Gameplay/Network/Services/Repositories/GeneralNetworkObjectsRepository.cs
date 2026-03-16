using System;
using _Project.Scripts.Common.Repositories.Base;
using Fusion;
using Behaviour = Fusion.Behaviour;
using Object = UnityEngine.Object;

namespace _Project.Scripts.Gameplay.Network.Services.Repositories
{
    public class GeneralNetworkObjectsRepository : IRepository<Fusion.Behaviour>
    {
        public NetworkRunner CurrentNetworkRunner { get; private set; }
        public NetworkSceneManagerDefault CurrentNetworkSceneManager { get; private set; }
        
        public T Add<T>(T networkObject) where T : Behaviour
        {
            if (CurrentNetworkRunner && CurrentNetworkSceneManager)
                throw new Exception("All the objects are already installed!");
            if (networkObject is NetworkRunner networkRunner)
            {
                if (CurrentNetworkRunner)
                    throw new Exception("Only one NetworkRunner is allowed!");
                CurrentNetworkRunner = networkRunner;
                return networkObject;
            } 
            if (networkObject is NetworkSceneManagerDefault networkSceneManager)
            {
                if (CurrentNetworkSceneManager)
                    throw new Exception("Only one NetworkSceneManagerDefault is allowed!");
                CurrentNetworkSceneManager = networkSceneManager;
                return networkObject;
            }
            throw new Exception("The object is not defined!");
        }
        
        public void DestroyNetworkRunnerAndSceneManager()
        {
            Object.Destroy(CurrentNetworkRunner);
            Object.Destroy(CurrentNetworkSceneManager);
            CurrentNetworkRunner = null;
            CurrentNetworkSceneManager = null;
        }
    }
}
