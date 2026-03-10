using Cysharp.Threading.Tasks;

namespace _Project.Scripts.View.Base
{
    public interface IWindow
    {
        bool IsOpen { get; }
        bool IsInteractable { get; }
        UniTask OpenAsync();
        UniTask CloseAsync();
    }
}