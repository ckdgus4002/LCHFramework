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
            if (status == TaskStatus.RanToCompletion) return TaskUtility.CompletedTask.False;
            if (status == TaskStatus.Canceled) return TaskUtility.CompletedTask.True;
            return new Task<bool>(() => task.Status == TaskStatus.Canceled, new TaskCanceledException(task).CancellationToken);
        }
    }
}