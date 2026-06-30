using System.Collections.Generic;
using _Project.Scripts.Common.SceneSwitcher;
using _Project.Scripts.Gameplay.Network.Services.Factories;
using _Project.Scripts.Gameplay.Network.Services.Factories.NetworkObjectProvider;
using _Project.Scripts.Gameplay.Network.Services.Repositories;
using Fusion;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public abstract class BaseNetworkInstaller : AdvancedMonoInstaller
    {
        private GameStateSwitcher _gameStateSwitcher;

        protected bool IsServer
        {
            get
            {
                if (_gameStateSwitcher.StartGameArgs.GameMode == GameMode.Host)
                    return true;
                return false;
            }
        }
        
        [Inject] private void Construct(GameStateSwitcher gameStateSwitcher) => 
            _gameStateSwitcher = gameStateSwitcher;
    }
}