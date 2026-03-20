using System;
using System.Collections.Generic;
using _Project.Scripts.Configs.Base;
using _Project.Scripts.Configs.Services.Base;

namespace _Project.Scripts.Configs
{
    public class ScriptableObjectsConfigService : IConfigService
    {
        private readonly List<IConfig> _configs;

        public ScriptableObjectsConfigService(List<IConfig> configs) => 
            _configs = configs;

        public T GetConfig<T>() where T : IConfig
        {
            foreach (IConfig config in _configs)
            {
                if (config is T concreteConfig) 
                    return concreteConfig;
            }

            throw new Exception("Config not found!");
        }
    }
}