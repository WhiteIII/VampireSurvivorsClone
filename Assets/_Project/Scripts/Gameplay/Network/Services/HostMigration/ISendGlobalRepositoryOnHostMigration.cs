using _Project.Scripts.Common.Services.Repositories.Implementation;

namespace _Project.Scripts.Gameplay.Network.Services.HostMigration
{
    public interface ISendGlobalRepositoryOnHostMigration
    {
        void OnHostMigration(GlobalRepository globalRepository);
    }
}