using System.Collections.Generic;
using _Project.Scripts.ViewModel.Base;
using Cysharp.Threading.Tasks;
using R3;

namespace _Project.Scripts.ViewModel.Implementation
{
    public class LoadingWindowViewModel : IViewModel
    {
        public ReadOnlyReactiveProperty<float> Progress => _progress;
        
        private readonly ReactiveProperty<float>  _progress = new();

        public async UniTask StartLoadingAsync(UniTask[] tasks)
        {
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
        
        public void ResetLoadingProgress() => 
            _progress.Value = 0;
    }
}