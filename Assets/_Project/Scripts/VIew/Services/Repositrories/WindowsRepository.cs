using System.Collections.Generic;
using _Project.Scripts.Common.Repositories.Base;
using _Project.Scripts.Gameplay.Services.Repositories;
using _Project.Scripts.VIew.Base;
using UnityEngine;

namespace _Project.Scripts.VIew.Services.Repositrories
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
    }
}
