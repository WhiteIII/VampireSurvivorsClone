using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace _Project.Scripts.VIew.Animation.Services
{
    public class FadeAnimation
    {   
        private readonly CanvasGroup _canvasGroup;
        private readonly float _duration;
        
        public FadeAnimation(CanvasGroup canvasGroup, float duration)
        {
            _canvasGroup = canvasGroup;
            _duration = duration;
        }
        
        public async UniTask PlayAnimationAsync(float from, float to, CancellationToken cancellationToken = default) 
        {
            _canvasGroup.alpha = from;
            if (cancellationToken.IsCancellationRequested == false)
                await _canvasGroup.DOFade(to, _duration).AsyncWaitForCompletion();
        }
    }
}