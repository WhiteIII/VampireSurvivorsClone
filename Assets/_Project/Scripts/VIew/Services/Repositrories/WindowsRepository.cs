using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Common.Repositories.Base;
using _Project.Scripts.View.Base;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace _Project.Scripts.View.Services.Repositrories
{
    public class WindowsRepository : IRepository<Window>
    {
        private readonly List<Window> _windows = new();

        public bool TryGet<T>(out T resultWindow)
            where T : Window
        {
            resultWindow = null;
            foreach (Window window in _windows)
            {
                if (window is T concreteWindow)
                {
                    resultWindow = concreteWindow;
                    return true;
                }
            }
            return false;
        }
        
        public T Add<T>(T window)
            where T : Window
        {
            _windows.Add(window);
            return window;
        }

        public async UniTask CloseAndDestroyWindows(params Type[] windowTypes)
        {
            if (windowTypes.Length == 0)
                return;
            Queue<Window> windowsToRemove = new(windowTypes.Length);
            foreach (Window window in _windows)
            {
                foreach (Type type in windowTypes)
                {
                    if (window.GetType() != type)
                        continue;
                    await window.CloseAsync();
                    windowsToRemove.Enqueue(window);
                    Object.Destroy(window.gameObject);
                }
            }
            while (windowsToRemove.Count > 0)
                _windows.Remove(windowsToRemove.Dequeue());
        }

        public void DisableInteractableOnWindows(params Type[] types) => 
            InvokeWindowMethodIf(
                types, 
                window => window.DisableInteractable(), 
                (window, type) => window.GetType() == type);
        
        public void EnableInteractableOnWindows(params Type[] types) => 
            InvokeWindowMethodIf(
                types, 
                window => window.EnableInteractable(), 
                (window, type) => window.GetType() == type);

        private void InvokeWindowMethodIf(Type[] types, Action<Window> action, Func<Window, Type, bool> predicate)
        {
            _windows.ForEach(window => types.ToList().ForEach(type =>
            {
                if (predicate(window, type))
                    action(window);
            }));
        }
    }
}