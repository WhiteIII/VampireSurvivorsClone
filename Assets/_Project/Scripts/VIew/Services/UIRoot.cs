using _Project.Scripts.View.Base;
using UnityEngine;

namespace _Project.Scripts.View.Services
{
    public class UIRoot
    {
        private readonly RectTransform _iuRootRectTransform;

        public UIRoot(RectTransform iuRootRectTransform) => 
            _iuRootRectTransform = iuRootRectTransform;

        public T Add<T>(T window)
            where T : Window
        {
            window.transform.SetParent(_iuRootRectTransform);
            return window;
        } 
    }
}