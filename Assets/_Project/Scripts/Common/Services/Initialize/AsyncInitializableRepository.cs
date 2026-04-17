using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Common.Services.Initialize
{
    public class AsyncInitializableRepository
    {
        private readonly List<IAsyncInitializable> _initializables;

        public AsyncInitializableRepository(List<IAsyncInitializable> initializables) => 
            _initializables = initializables;

        public UniTask[] GetTasks()
        {
            UniTask[] result = new UniTask[_initializables.Count];
            for (int i = 0; i < _initializables.Count; i++)
                result[i] = _initializables[i].InitializeAsync();
            return result;
        }
    }
}