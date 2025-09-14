using System;
using System.Threading.Tasks;
using LCHFramework.Utilities;
using Debug = LCHFramework.Utilities.Debug;

namespace LCHFramework.Extensions
{
    public static class TaskExtension
    {
        public static async void Forget(this Task task, bool logException = true)
        {
            try
            {
                await task;
            }
            catch (Exception e)
            {
                if (logException) Debug.LogException(e);
            }
        }
        
        public static Task<bool> SuppressCancellationThrow(this Task task)
        {
            var status = task.Status;
            if (status == TaskStatus.RanToCompletion) return Task.FromResult(false);
            if (status == TaskStatus.Canceled) return Task.FromResult(true);
            return new Task<bool>(() => task.Status == TaskStatus.Canceled, new TaskCanceledException(task).CancellationToken);
        }
    }
}