using System.Threading;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.VIew.Base
{
    public interface IWindowAnimation
    {
        UniTask PlayOpenAnimationAsync(CancellationToken cancellationToken = default);
        UniTask PlayCloseAnimationAsync(CancellationToken cancellationToken = default);
    }
}