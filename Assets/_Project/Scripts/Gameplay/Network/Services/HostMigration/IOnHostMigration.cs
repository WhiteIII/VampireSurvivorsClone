using _Project.Scripts.Gameplay.Network.Services.Repositories;

namespace _Project.Scripts.Gameplay.Network.Services.HostMigration
{
    public interface IOnHostMigration
    {
        void OnHostMigration(GeneralNetworkObjectsRepository generalNetworkObjectsRepository);
    }
}