using System;
using System.Collections.Generic;
using _Project.Scripts.Common.Services.Factories.ObjectPools.Base;
using _Project.Scripts.Gameplay.Network.Services.BaseComponent;
using _Project.Scripts.ViewModel.Base;
using R3;
using UnityEngine;

namespace _Project.Scripts.ViewModel.Implementation
{
    public class EnemiesBarsViewModel : IViewModel, IDisposable
    {
        public readonly Observable<EnemyBarViewModel> OnEnemyBarAdded;
        public readonly Observable<EnemyBarViewModel> OnEnemyBarRemoved;
        
        private readonly CompositeDisposable _disposable = new();
        private readonly ObjectPoolWithSubjects<EnemyBarViewModel, Enemy> _pool;

        public EnemiesBarsViewModel()
        {
            _pool = new ObjectPoolWithSubjects<EnemyBarViewModel, Enemy>(
                () => new EnemyBarViewModel(),
                OnGet,
                OnRelease);
            
            OnEnemyBarAdded = _pool.OnGet;
            OnEnemyBarRemoved = _pool.OnRelease;
        }

        public void SetObservables(Observable<Enemy> onRevive, Observable<Enemy> onDead)
        {
            onRevive
                .Subscribe(Add)
                .AddTo(_disposable);
            onDead
                .Subscribe(Remove)
                .AddTo(_disposable);
        }
        
        public void Dispose() => 
            _disposable.Dispose();

        private void Add(Enemy enemy) => 
            _pool.Get<EnemyBarViewModel>(enemy);

        private void Remove(Enemy enemy) => 
            _pool.ReleaseByParameter<EnemyBarViewModel>(enemy);

        private void OnGet(EnemyBarViewModel viewModel, Enemy enemy)
        {
            viewModel.SetHealthObservables(enemy.OnHealthChanged, enemy.OnMaxHealthChanged);
            viewModel.SetEnemyPositionObservable(enemy.Position);
        }

        private void OnRelease(EnemyBarViewModel viewModel, Enemy _) => 
            viewModel.Reset();
    }
}