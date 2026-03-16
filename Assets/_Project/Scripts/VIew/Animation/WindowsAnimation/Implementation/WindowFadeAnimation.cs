using System.Threading;
using _Project.Scripts.VIew.Animation.Services;
using _Project.Scripts.View.Base;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.VIew.Animation.WindowsAnimation.Implementation
{
    [RequireComponent(typeof(CanvasGroup))]
    public class WindowFadeAnimation : MonoBehaviour, IWindowAnimation
    {
        [SerializeField] private float _duration = 0.4f;

        private FadeAnimation _fadeAnimation;
        
        private void Awake() =>
            _fadeAnimation = new FadeAnimation(GetComponent<CanvasGroup>(), _duration);

        public UniTask PlayCloseAnimationAsync(CancellationToken cancellationToken = default) => 
            _fadeAnimation.PlayAnimationAsync(1f, 0f, cancellationToken);

        public UniTask PlayOpenAnimationAsync(CancellationToken cancellationToken = default) => 
            _fadeAnimation.PlayAnimationAsync(0f, 1f, cancellationToken);
    }
}