using _Project.Scripts.Common.Services.Initialize;

namespace _Project.Scripts.CompositionRoot.Services
{
    public interface IAsyncDependence<out T> : IAsyncInitializable
        where T : class
    {
        bool InstanceCreated { get; }
        bool CreatedInProcess { get; }
        T Instance { get; }
    }
}