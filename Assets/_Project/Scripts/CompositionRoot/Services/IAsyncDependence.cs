using _Project.Scripts.Common.Services.Initialize;

namespace _Project.Scripts.CompositionRoot.Services
{
    public interface IAsyncDependence : IAsyncInitializable
    {
        bool InstanceCreated { get; }
        bool CreatedInProcess { get; }
        object ObjectInstance { get; }
    }
}