using _Project.Scripts.Common.Services.Initialize;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.CompositionRoot.Services
{
    public interface IAsyncDependence : IAsyncInitializable
    {
        UniTask Task { get; }
        object ObjectInstance { get; }
    }
}