namespace _Project.Scripts.Configs.Base
{
    public interface IGameConfig : IConfig
    {
        int MaxPlayerCountInSession { get; }
    }
}