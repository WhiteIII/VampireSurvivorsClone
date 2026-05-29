using _Project.Scripts.Configs;
using _Project.Scripts.Configs.Base;
using _Project.Scripts.Configs.Implementation.ScriptableObjects;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.CompositionRoot.Installers.SceneContext
{
    public class ConfigsInstaller : MonoInstaller
    {
        [Header("Configs:")]
        [SerializeField] private GameConfigScriptableObject _gameConfig;
        [SerializeField] private PlayerDataScriptableObject _playerData;
        
        public override void InstallBindings()
        {
            BindConfig(_gameConfig);
            BindConfig(_playerData);
            
            Container.BindInterfacesTo<ScriptableObjectsConfigService>().AsSingle();
        }
        
        private void BindConfig<T>(T config) where T : IConfig => 
            Container.Bind<IConfig>().FromInstance(config).AsCached();
    }
}