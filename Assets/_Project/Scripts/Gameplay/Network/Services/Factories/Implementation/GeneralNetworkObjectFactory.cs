using _Project.Scripts.Gameplay.Network.Services.Factories.Creators.Implementation;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Factories.Implementation
{
    public class GeneralNetworkObjectFactory<T> : IFactory<T>
        where T : Fusion.Behaviour
    {
        private readonly GeneralNetworkObjectsCreator _creator;

        public GeneralNetworkObjectFactory(GeneralNetworkObjectsCreator creator) => 
            _creator = creator;

        public T Create() => 
            _creator.CreateComponentOnGameObject<T>();
    }
}