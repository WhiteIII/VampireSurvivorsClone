using _Project.Scripts.Configs.Base;

namespace _Project.Scripts.Configs.Services.Base
{
    public interface IConfigService
    {
        T GetConfig<T>() where T : IConfig;
    }
}