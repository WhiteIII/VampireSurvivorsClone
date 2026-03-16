using System;
using System.Collections.Generic;
using System.Threading;
using _Project.Scripts.VIew.Animation.Services;
using _Project.Scripts.View.Base;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.VIew.Animation.WindowsAnimation.Implementation
{
    public partial class WindowMoveAnimation : MonoBehaviour, IWindowAnimation
    {
        [Header("WindowElements:")] 
        [SerializeField] private MoveAnimationConfig[] _configs;

        [Header("Settings:")] 
        [SerializeField] private float _duration = 0.2f;
        [SerializeField] private float _cooldown = 0.05f;

        private void Awake()
        {
            foreach (MoveAnimationConfig config in _configs)
                config.MoveAnimation = new MoveAnimation(config.RectTransform, _duration);
        }

        public async UniTask PlayCloseAnimationAsync(CancellationToken cancellationToken = default)
        {
            List<UniTask> tasks = new();
            foreach (MoveAnimationConfig animationConfig in _configs)
            {
                tasks.Add(animationConfig
                    .MoveAnimation
                    .PlayAnimationAsync(animationConfig.To, animationConfig.From, cancellationToken));
                await UniTask.WaitForSeconds(_cooldown, false, PlayerLoopTiming.Update, cancellationToken);
            }

            await UniTask.WhenAll(tasks);
        }

        public async UniTask PlayOpenAnimationAsync(CancellationToken cancellationToken = default)
        {
            List<UniTask> tasks = new();
            foreach (MoveAnimationConfig animationConfig in _configs)
            {
                tasks.Add(animationConfig
                    .MoveAnimation
                    .PlayAnimationAsync(animationConfig.From, animationConfig.To, cancellationToken));
                await UniTask.WaitForSeconds(_cooldown, false, PlayerLoopTiming.Update, cancellationToken);
            }

            await UniTask.WhenAll(tasks);
        }
    }
    
    public partial class WindowMoveAnimation
    {
        [Serializable]
        private class MoveAnimationConfig
        {
            public MoveAnimation MoveAnimation;

            [field: SerializeField] public Vector2 From { get; private set; }
            [field: SerializeField] public Vector2 To { get; private set; }
            [field: SerializeField] public RectTransform RectTransform { get; private set; }
        }
    }
}
