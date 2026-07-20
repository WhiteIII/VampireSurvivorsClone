using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using R3;
using Zenject;

namespace _Project.Scripts.Gameplay.Network
{
    public class PlayersInSessionData : IEnumerable<PlayerRef>, IInitializable, IDisposable
    {
        public Observable<(NetworkRunner, PlayerRef)> OnPlayerJoinedSubject => _listener.OnPlayerJoinedSubject;
        public Observable<(NetworkRunner, PlayerRef)> OnPlayerLeftSubject => _listener.OnPlayerLeftSubject;
        
        private readonly NetworkRunnerCallBacksListener _listener;
        private readonly List<PlayerRef> _players = new();
        private readonly CompositeDisposable _disposables = new();

        public PlayersInSessionData(NetworkRunnerCallBacksListener listener) => 
            _listener = listener;

        public void Initialize()
        {
            OnPlayerJoinedSubject
                .Subscribe(x => _players.Add(x.Item2))
                .AddTo(_disposables);
            OnPlayerLeftSubject
                .Subscribe(x => _players.Remove(x.Item2))
                .AddTo(_disposables);
        }

        public void Dispose() => 
            _disposables.Dispose();

        public IEnumerator<PlayerRef> GetEnumerator() => 
            _players.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => 
            GetEnumerator();
    }
}