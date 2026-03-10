using System.Threading;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.View.Base
{
    public interface IWindowAnimation
    {
        UniTask PlayOpenAnimationAsync(CancellationToken cancellationToken = default);
        UniTask PlayCloseAnimationAsync(CancellationToken cancellationToken = default);
    }
}