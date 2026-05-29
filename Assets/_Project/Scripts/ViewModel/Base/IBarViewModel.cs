using R3;

namespace _Project.Scripts.ViewModel.Base
{
    public interface IBarViewModel : IViewModel
    {
        Observable<float> OnValueChanged { get; }
    }
}