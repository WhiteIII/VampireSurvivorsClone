using System;
using System.Collections.Generic;
using _Project.Scripts.View.Base;
using _Project.Scripts.View.Services.Repositrories;
using Cysharp.Threading.Tasks;
using Zenject;

namespace _Project.Scripts.View.Services
{
    public class UIController 
    {
        private readonly List<IFactory<Window>> _windowFactories;
        private readonly WindowsRepository _repository;
        private readonly UIRoot _uiRoot;

        public UIController(List<IFactory<Window>> windowFactories, WindowsRepository repository, UIRoot uiRoot)
        {
            _windowFactories = windowFactories;
            _repository = repository;
            _uiRoot = uiRoot;
        }

        public async UniTask CreateAndOpenWindowAsync<T>()
            where T : Window
        {
            if (_repository.TryGet(out T window))
            {
                await window.OpenAsync();
                return;
            }

            await _uiRoot.Add(GetWindowFactory<T>().Create()).OpenAsync();
        }

        public async UniTask OpenWindowAsync<T>()
            where T : Window
        {
            if (_repository.TryGet(out T window) == false)
                throw new Exception("Window not found!");
            if (window.IsOpen == false)
                await window.OpenAsync();
        }
        
        public async UniTask CloseWindowAsync<T>() 
            where T : Window
        {
            if (_repository.TryGet(out T window) == false)
                throw new Exception("Window not found!");
            if (window.IsOpen)
                await window.CloseAsync();
        }
        
        public async UniTask DestroyAndCloseWindowsAsync(params Type[] windows)
        {
            _repository.DisableInteractableOnWindows(windows);
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