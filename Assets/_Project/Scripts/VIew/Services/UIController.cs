using _Project.Scripts.View.Base;
using _Project.Scripts.View.Services.Repositrories;
using Zenject;

namespace _Project.Scripts.View.Services
{
    public class UIController 
    {
        private readonly IFactory<IWindow> _windowFactory;
        private readonly WindowsRepository _repository;

    }
}