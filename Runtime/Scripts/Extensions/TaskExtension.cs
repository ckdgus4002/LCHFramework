using System;
using System.Threading.Tasks;
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
        
        public static async Task SuppressCancellationThrow(this Task task)
        {
            try
            {
                await task;
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}