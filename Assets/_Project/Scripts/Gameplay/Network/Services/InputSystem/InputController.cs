using System;
using Fusion;
using R3;
using UnityEngine;
using Zenject;
using static UnityEngine.Input;

namespace _Project.Scripts.Gameplay.Network.Services.InputSystem
{
    public class InputController : IInitializable, IDisposable
    {
        private const string HORIZONTAL = "Horizontal";
        private const string VERTICAL = "Vertical";
        
        private readonly NetworkRunnerCallBacksListener _callbacks;
        private readonly CompositeDisposable _disposables = new();

        public InputController(NetworkRunnerCallBacksListener callbacks) => 
            _callbacks = callbacks;

        public void Initialize()
        {
            _callbacks
                .OnInputSubject
                .Subscribe(x=> OnInput(x.Item1, x.Item2))
                .AddTo(_disposables);
        }

        public void Dispose() => 
            _disposables.Dispose();

        private void OnInput(NetworkRunner _, NetworkInput input)
        {
            InputData data = new()
            {
                Direction = new Vector3(GetAxis(HORIZONTAL), 0f, GetAxis(VERTICAL))
            };
            input.Set(data);
        }
    }
}