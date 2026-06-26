using System;
using System.Collections.Generic;
using _Project.Scripts.Common.Services.Factories.ObjectPools.Base;
using _Project.Scripts.ViewModel.Base;
using R3;
using UnityEngine;

namespace _Project.Scripts.ViewModel.Implementation
{
    public class EnemiesBarsViewModel : IViewModel, IDisposable
    {
        public readonly Observable<EnemyBarViewModel> OnEnemyBarAdded;
        public readonly Observable<EnemyBarViewModel> OnEnemyBarRemoved;
        
        private readonly ObjectPoolWithSubjects<EnemyBarViewModel, EnemyBarViewModelData> _pool;
        private readonly List<EnemyBarViewModelData> _enemiesData = new();
        
        public EnemiesBarsViewModel()
        {
            _pool = new ObjectPoolWithSubjects<EnemyBarViewModel, EnemyBarViewModelData>(
                () => new EnemyBarViewModel(),
                OnGet,
                OnRelease);
            
            OnEnemyBarAdded = _pool.OnGet;
            OnEnemyBarRemoved = _pool.OnRelease;
        }

        public void Dispose()
        {
            
        }

        public void Add(EnemyBarViewModelData enemyBarViewModelData)
        {
            _enemiesData.Add(enemyBarViewModelData);
            _pool.Get<EnemyBarViewModel>(enemyBarViewModelData);
        }

        public void Remove(uint id)
        {
            EnemyBarViewModelData data = GetDataById(id);
            _enemiesData.Remove(data);
            _pool.ReleaseByParameter<EnemyBarViewModel>(data);
        }

        private EnemyBarViewModelData GetDataById(uint id)
        {
            foreach (EnemyBarViewModelData data in _enemiesData)
            {
                if (data.EnemyId == id)
                    return data;
            }
            throw new Exception("Data not found!");
        }

        private void OnGet(EnemyBarViewModel viewModel, EnemyBarViewModelData data)
        {
            viewModel.SetHealthObservables(data.OnHealthChanged, data.OnMaxHealthChanged);
            viewModel.SetEnemyPositionObservable(data.OnPositionChanged);
        }

        private void OnRelease(EnemyBarViewModel viewModel, EnemyBarViewModelData _) => 
            viewModel.Reset();
    }

    public struct EnemyBarViewModelData
    {
        public Observable<Vector3> OnPositionChanged;
        public ReadOnlyReactiveProperty<int> OnHealthChanged;
        public ReadOnlyReactiveProperty<int> OnMaxHealthChanged;
        public uint EnemyId;
    }
}