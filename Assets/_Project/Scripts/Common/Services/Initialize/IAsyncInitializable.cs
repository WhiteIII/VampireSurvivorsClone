using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Common.Services.Initialize
{
    public interface IAsyncInitializable
    {
        UniTask InitializeAsync();
    }
}