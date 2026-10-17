using System;
using System.Threading.Tasks;

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
                if (logException) UnityEngine.Debug.LogException(e);
            }
        }
        
        public static async void Forget<T>(this Task<T> task, bool logException = true)
        {
            try
            {
                await task;
            }
            catch (Exception e)
            {
                if (logException) UnityEngine.Debug.LogException(e);
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
        
        public static async Task<T> SuppressCancellationThrow<T>(this Task<T> task)
        {
            try
            {
                return await task;
            }
            catch (OperationCanceledException)
            {
            }

            return default;
        }
    }
}