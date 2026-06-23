using _Project.Scripts.Common.Services.Factories.ObjectPools.Base;
using _Project.Scripts.ViewModel.Base;
using R3;
using UnityEngine;

namespace _Project.Scripts.ViewModel.Implementation
{
    public class EnemiesBarsViewModel : IViewModel
    {
        public readonly Observable<EnemyBarViewModel> OnEnemyBarAdded;
        public readonly Observable<EnemyBarViewModel> OnEnemyBarRemoved;
        
        private readonly ObjectPoolWithSubjects<EnemyBarViewModel, EnemyBarViewModelData> _pool;
        
        public EnemiesBarsViewModel()
        {
            OnEnemyBarAdded = _pool.OnGet;
            OnEnemyBarRemoved = _pool.OnRelease;
        }

        public void Add(EnemyBarViewModelData enemyBarViewModelData)
        {
            
        }

        public void Remove(string id)
        {
            
        }
    }

    public struct EnemyBarViewModelData
    {
        public Observable<Vector3> OnPositionChanged;
        public ReadOnlyReactiveProperty<int> OnHealthChanged;
        public ReadOnlyReactiveProperty<int> OnMaxHealthChanged;
        public string EnemyId;
    }
}