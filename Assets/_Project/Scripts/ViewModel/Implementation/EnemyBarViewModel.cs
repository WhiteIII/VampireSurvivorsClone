using R3;
using UnityEngine;

namespace _Project.Scripts.ViewModel.Implementation
{
    public class EnemyBarViewModel : BarViewModel
    {
        public Observable<Vector3> OnPositionChanged { get; private set; }
        
        public void SetEnemyPositionObservable(Observable<Vector3> positionObservable) => 
            OnPositionChanged = positionObservable;
    }
}