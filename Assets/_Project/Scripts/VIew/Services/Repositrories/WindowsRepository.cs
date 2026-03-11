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
            int windowTypesIndex = 0;
            Queue<Window> windowsToRemove = new(windowTypes.Length);
            foreach (Window window in _windows)
            {
                if (window.GetType() == windowTypes[windowTypesIndex])
                {
                    windowTypesIndex++;
                    await window.CloseAsync();
                    windowsToRemove.Enqueue(window);
                    Object.Destroy(window);
                    if (windowTypesIndex == windowTypes.Length)
                        break;
                }
            }
            while (windowsToRemove.Count > 0)
                _windows.Remove(windowsToRemove.Dequeue());
        }
        
        public void DisableInteractableAllWindows() => 
            InvokeAllWindowsMethods(window => window.DisableInteractable());
        
        public void EnableInteractableAllWindows() => 
            InvokeAllWindowsMethods(window => window.EnableInteractable());
        
        //public void DisableInteractableAllWindowsWithout(params Type[] types) => 
          //  InvokeWindowMethodIf(types, window => window.DisableInteractable());
        
        //public void EnableInteractableAllWindowsWithout(params Type[] types) => 
          //  InvokeWindowMethodIf(types, window => window.EnableInteractable(), );

        public void DestroyAllWindows()
        {
            foreach (Window window in _windows)
                Object.Destroy(window);
            _windows.Clear();
        }

        private void InvokeAllWindowsMethods(Action<Window> action) => 
            _windows.ForEach(action);
        
        private void InvokeWindowMethodIf(Type[] types, Action<Window> action, Predicate<Window> predicate)
        {
            _windows.ForEach(window => types.ToList().ForEach(type =>
            {
                if (predicate(window))
                    action(window);
            }));
        }
    }
}