using System.Threading;
using System.Threading.Tasks;
using LCHFramework.Utilities;

namespace LCHFramework.Extensions
{
    public static class TaskExtension
    {
        public static Task<bool> SuppressCancellationThrow(this Task task)
        {
            var status = task.Status;
            if (status == TaskStatus.RanToCompletion) return TaskUtility.CompletedTasks.False;
            if (status == TaskStatus.Canceled) return TaskUtility.CompletedTasks.True;
            return task.ToCanceledTask();
        }

        public static Task<bool> ToCanceledTask(this Task task) => new(() => task.Status == TaskStatus.Canceled, task.GetToken());
        
        public static CancellationToken GetToken(this Task task) => new TaskCanceledException(task).CancellationToken;
    }
}