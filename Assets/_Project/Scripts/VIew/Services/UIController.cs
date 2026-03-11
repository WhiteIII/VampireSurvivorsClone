using System;
using _Project.Scripts.View.Base;
using _Project.Scripts.View.Services.Repositrories;
using Cysharp.Threading.Tasks;
using Zenject;

namespace _Project.Scripts.View.Services
{
    public class UIController 
    {
        private readonly IFactory<Window>[] _windowFactories;
        private readonly WindowsRepository _repository;

        public UIController(IFactory<Window>[] windowFactories, WindowsRepository repository)
        {
            _windowFactories = windowFactories;
            _repository = repository;
        }

        public async UniTask CreateAndOpenWindowAsync<T>()
            where T : Window
        {
            if (_repository.TryGet(out T window))
            {
                await window.OpenAsync();
                return;
            }
            await GetWindowFactory<T>().Create().OpenAsync();
        }

        public async UniTask DestroyAndCloseWindowsAsync(params Type[] windows)
        {
            
            await _repository.CloseAndDestroyWindows(windows);
        } 
        
        private IFactory<T> GetWindowFactory<T>()
            where T : Window
        {
            foreach (IFactory<Window> windowFactory in _windowFactories)
            {
                if (windowFactory is IFactory<T> resultFactory)
                    return resultFactory;
            }
            return null;
        }
    }
}