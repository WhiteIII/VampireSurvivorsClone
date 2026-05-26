namespace _Project.Scripts.Configs.Base
{
    public interface IPlayerData : IConfig
    {
        int Health { get; }
        int Damage { get; }
        float AttackCooldown { get; }
        float MovementSpeed { get; }
        float AttackDistance { get; }
    }
}