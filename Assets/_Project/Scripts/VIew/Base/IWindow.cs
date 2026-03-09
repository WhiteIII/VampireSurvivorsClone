using Cysharp.Threading.Tasks;

namespace _Project.Scripts.VIew.Base
{
    public interface IWindow
    {
        UniTask OpenAsync();
        UniTask CloseAsync();
    }
}