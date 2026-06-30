using System;
using System.Collections.Generic;
using _Project.Scripts.ViewModel.Base;
using Cysharp.Threading.Tasks;
using R3;

namespace _Project.Scripts.ViewModel.Implementation
{
    public partial class LoadingWindowViewModel : IViewModel
    {
        public ReadOnlyReactiveProperty<float> Progress => _progress;
        
        private readonly ReactiveProperty<float>  _progress = new();

        public async UniTask StartLoadingAsync(params UniTask[] tasks)
        {
            if (MultiStageLoadingInProgress)
                throw new Exception("Multi stage loading in progress!");
            
            List<UniTask> tasksList = new(tasks);
            int tasksCount = tasksList.Count;
            float succeededTasksCount = 0;
            
            while (tasksList.Count > 0)
            {
                if (CheckUniTaskSuccess(tasksList, out List<UniTask> succeededTasks))
                {
                    RemoveSucceededTasksFromTasks(tasksList, succeededTasks);
                    succeededTasksCount += succeededTasks.Count;
                    _progress.Value = tasksCount / succeededTasksCount;
                }
                await UniTask.Yield();
            }
        }

        private void RemoveSucceededTasksFromTasks(List<UniTask> tasks, IEnumerable<UniTask> succeededTasks)
        {
            foreach (UniTask task in succeededTasks)
                tasks.Remove(task);
        }
        
        private bool CheckUniTaskSuccess(IEnumerable<UniTask> tasks, out List<UniTask> succeededTasks)
        {
            succeededTasks = null;
            foreach (UniTask task in tasks)
            {
                if (task.Status == UniTaskStatus.Succeeded)
                {
                    if  (succeededTasks == null)
                        succeededTasks = new List<UniTask>();
                    succeededTasks.Add(task);
                }
            }

            if (succeededTasks == null)
                return false;
            return true;
        }

        public void ResetLoadingProgress()
        {
            _progress.Value = 0;
            _multiStageLoading = null;
        } 
    }

    public partial class LoadingWindowViewModel
    {
        private MultiStageLoading _multiStageLoading;

        public bool MultiStageLoadingInProgress
        {
            get
            {
                if (_multiStageLoading == null)
                    return false;
                return _multiStageLoading.InProgress;
            }
        }

        public void StartMultiStageLoading(int totalTasksCount) => 
            _multiStageLoading = new MultiStageLoading(totalTasksCount);

        public async UniTask WaitLoadingForMultiStageLoadingAsync(params UniTask[] tasks)
        {
            if (MultiStageLoadingInProgress == false)
                throw new Exception("Multi stage loading in not progress!");

            _progress.Value = await _multiStageLoading.WaitLoadingAndCheckMultistageLoadingStatusAsync(tasks);
        }

        private class MultiStageLoading
        {
            private readonly int _totalCount;
            private int _currentCount;
            
            public bool InProgress { get; private set; } = true;
            
            public MultiStageLoading(int totalCount) => 
                _totalCount = totalCount;
            
            public async UniTask<float> WaitLoadingAndCheckMultistageLoadingStatusAsync(params UniTask[] tasks)
            {
                if (InProgress == false)
                    InProgress = true;
                _currentCount += tasks.Length;
                if (_currentCount > _totalCount)
                    throw new Exception("Too many tasks!");
                
                while (CheckTasksComplete(tasks) == false)
                    await UniTask.Yield();
                
                if (_currentCount == _totalCount)
                    InProgress = false;
                
                return (float)_currentCount / _totalCount;
            }

            private bool CheckTasksComplete(UniTask[] tasks)
            {
                foreach (UniTask task in tasks)
                {
                    if (task.Status == UniTaskStatus.Pending)
                        return false;
                }
                return true;
            }
        }
    }
}