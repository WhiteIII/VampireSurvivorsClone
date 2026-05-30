using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Common.Services.Repositories.Base;
using R3;

namespace _Project.Scripts.ViewModel.Implementation
{
    public class EnemyBarsViewModelRepository : IRepository<EnemyBarViewModel>
    {
        public Observable<EnemyBarViewModel> OnEnemyBarAdded => _onEnemyBarAdded;
        public Observable<EnemyBarViewModel> OnEnemyBarRemoved => _onEnemyBarRemoved;
        
        private readonly List<EnemyBarViewModel> _enemiesBars = new();
        private readonly Subject<EnemyBarViewModel> _onEnemyBarAdded = new();
        private readonly Subject<EnemyBarViewModel> _onEnemyBarRemoved = new();
        
        public int Count => _enemiesBars.Count;
        
        public bool TryGet<T>(out T foundedItem) where T : EnemyBarViewModel
        {
            foundedItem = null;
            
            if (_enemiesBars.Contains(foundedItem))
                return true;
            
            return false;
        }

        public T Add<T>(T item) where T : EnemyBarViewModel
        {
            _enemiesBars.Add(item);
            _onEnemyBarAdded.OnNext(item);
            return item;
        }

        public void Remove(EnemyBarViewModel item)
        {
            _onEnemyBarRemoved.OnNext(item);
            _enemiesBars.Remove(item);
        } 

        public IEnumerator<EnemyBarViewModel> GetEnumerator() => 
            _enemiesBars.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => 
            GetEnumerator();
    }
}