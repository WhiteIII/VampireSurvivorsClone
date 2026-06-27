using System.Threading;
using _Project.Scripts.View.Base;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.VIew.Animation.WindowsAnimation.Implementation
{
    public class GeneralWindowAnimation : MonoBehaviour, IWindowAnimation
    {
        public UniTask PlayOpenAnimationAsync(CancellationToken cancellationToken = default)
        {
            gameObject.SetActive(true);
            return UniTask.CompletedTask;
        }

        public UniTask PlayCloseAnimationAsync(CancellationToken cancellationToken = default)
        {
            gameObject.SetActive(false);
            return UniTask.CompletedTask;
        }
    }
}