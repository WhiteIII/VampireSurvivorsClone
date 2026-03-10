using System;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Common.Repositories.Base;
using _Project.Scripts.View.Base;
using Object = UnityEngine.Object;

namespace _Project.Scripts.View.Services.Repositrories
{
    public class WindowsRepository : IRepository<Window>
    {
        private readonly List<Window> _windows = new();

        public T Add<T>(T window)
            where T : Window
        {
            _windows.Add(window);
            return window;
        }

        public void DisableInteractableAllWindowsWithout(params Type[] types) => 
            InvokeIfWindowTypeNotIn(types, window => window.DisableInteractable());
        
        public void EnableInteractableAllWindowsWithout(params Type[] types) => 
            InvokeIfWindowTypeNotIn(types, window => window.EnableInteractable());

        public void DestroyAllWindows()
        {
            foreach (Window window in _windows)
                Object.Destroy(window);
            _windows.Clear();
        }

        private void InvokeIfWindowTypeNotIn(Type[] types, Action<Window> action)
        {
            _windows.ForEach(window => types.ToList().ForEach(type =>
            {
                if (window.GetType() != type)
                    action(window);
            }));
        }
    }
}