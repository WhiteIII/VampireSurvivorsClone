using System;
using _Project.Scripts.View.Implementation;
using _Project.Scripts.ViewModel.Base;
using R3;
using Zenject;

namespace _Project.Scripts.ViewModel.Implementation
{
    public class EnemiesBarsViewModel : IViewModel
    {
        public readonly Observable<EnemyBarViewModel> OnEnemyBarAdded;
        public readonly Observable<EnemyBarViewModel> OnEnemyBarRemoved;
        
        private readonly EnemyBarsViewModelRepository _repository;
        
        public EnemiesBarsViewModel(EnemyBarsViewModelRepository repository)
        {
            _repository = repository;
            OnEnemyBarAdded = _repository.OnEnemyBarAdded;
            OnEnemyBarRemoved = _repository.OnEnemyBarRemoved;
        }
    }
}