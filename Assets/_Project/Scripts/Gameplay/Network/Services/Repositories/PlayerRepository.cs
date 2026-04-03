using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Common.Services.Repositories.Base;
using _Project.Scripts.Configs.Base;
using _Project.Scripts.Configs.Services.Base;
using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using _Project.Scripts.Gameplay.Network.Services.Factories;
using Fusion;
using Zenject;

namespace _Project.Scripts.Gameplay.Network.Services.Repositories
{
    public class PlayerRepository : BaseNetworkObjectsRepository<Player>
    {

    }

    /*public class NetworkObjectBindHelper<T> : IFactory<T>
        where T : NetworkBehaviour
    {
        private readonly NetworkRunner _networkRunner;
        private readonly NetworkObjectsCreator _creator;
        
        public NetworkObjectBindHelper(
            GeneralNetworkObjectsRepository generalNetworkObjectsRepository, 
            NetworkObjectsCreator networkObjectsCreator)
        {
            _creator = networkObjectsCreator;
            _networkRunner = generalNetworkObjectsRepository.CurrentNetworkRunner;
        }

        public T Create()
        {
            //if (_networkRunner.)
        }
    }*/
}