using System;
using System.Collections.Generic;
using _Project.Scripts.View.Base;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.View.Services
{
    public class WindowSwitcher
    {
        private readonly List<IWindow> _windows = new();
        
        public WindowSwitcher(params IWindow[] windows) => 
            _windows.AddRange(windows);

        public async UniTask Switch<TFrom, TTo>()
            where TFrom : IWindow
            where TTo : IWindow
        {
            if (TryGetWindow(out TFrom fromWindow) == false)
                return;
            if (TryGetWindow(out TTo toWindow) == false)
                return;
            if (fromWindow.IsOpen)
                await fromWindow.CloseAsync();
            if (toWindow.IsOpen == false)
                await toWindow.OpenAsync();
        }

        private bool TryGetWindow<T>(out T result)
            where T : IWindow
        {
            result = default;
            if (_windows.Count == 0)
                throw new Exception("Window not found!");

            foreach (var window in _windows)
            {
                if (window is T windowAsT)
                {
                    result = windowAsT;
                    return true;
                }
            }
            throw new Exception("Window not found!");
        }
    }
}