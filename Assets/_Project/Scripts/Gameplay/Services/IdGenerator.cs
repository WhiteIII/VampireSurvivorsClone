using Fusion;

namespace _Project.Scripts.Gameplay.Services
{
    public class IdGenerator : NetworkBehaviour, IIdGenerator
    {
        [Networked] private uint FreeValue { get; set; }
        
        public uint GetId()
        {
            uint currentFreeValue = FreeValue;
            FreeValue++;
            return currentFreeValue;
        }
    }

    public interface IIdGenerator
    {
        uint GetId();
    }
}