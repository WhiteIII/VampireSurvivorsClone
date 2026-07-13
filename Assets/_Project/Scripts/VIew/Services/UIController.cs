using System;
using System.Collections.Generic;
using _Project.Scripts.Common.Services.Factories;
using _Project.Scripts.View.Base;
using _Project.Scripts.View.Services.Repositrories;
using Cysharp.Threading.Tasks;
using Zenject;

namespace _Project.Scripts.View.Services
{
    public class UIController
    {
        private readonly List<IAbstractOverAsyncFactory<Window>> _asyncWindowFactories;
        private readonly List<IFactory<Window>> _windowFactories;
        private readonly WindowsRepository _repository;
        private readonly UIRoot _uiRoot;

        public UIController(
            List<IFactory<Window>> windowFactories,
            WindowsRepository repository,
            UIRoot uiRoot,
            List<IAbstractOverAsyncFactory<Window>> asyncWindowFactories)
        {
            _windowFactories = windowFactories;
            _repository = repository;
            _uiRoot = uiRoot;
            _asyncWindowFactories = asyncWindowFactories;
        }

        public async UniTask CreateAndOpenWindowAsync<T>()
            where T : Window
        {
            if (_repository.TryGet(out T window))
            {
                await window.OpenAsync();
                return;
            }

            if (TryGetWindowFactory(out IFactory<T> windowFactory))
            {
                await _uiRoot.Add(windowFactory.Create()).OpenAsync();
                return;
            }
            if (TryGetAsyncWindowFactory(out IAbstractOverAsyncFactory<T> asyncWindowFactory) == false)
                throw new Exception("Can't create window!");
            await asyncWindowFactory.CreateAsync();
            await _uiRoot.Add(asyncWindowFactory.CreatedValue).OpenAsync();
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

        private bool TryGetWindowFactory<T>(out IFactory<T> factory)
            where T : Window
        {
            factory = GetWindowFactory<T>();
            if (factory == null)
                return false;
            return true;
        }

        private bool TryGetAsyncWindowFactory<T>(out IAbstractOverAsyncFactory<T> factory)
        {
            factory = GetAsyncWindowFactory<T>();
            if (factory == null)
                return false;
            return true;
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

        private IAbstractOverAsyncFactory<T> GetAsyncWindowFactory<T>()
        {
            foreach (IAbstractOverAsyncFactory<Window> windowFactory in _asyncWindowFactories)
            {
                if (windowFactory is IAbstractOverAsyncFactory<T> resultFactory)
                    return resultFactory;
            }
            return null;
        }
    }
}