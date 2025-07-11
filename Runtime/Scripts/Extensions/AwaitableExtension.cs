using System;
using UnityEngine;
using Debug = LCHFramework.Utilities.Debug;

namespace LCHFramework.Extensions
{
    public static class AwaitableExtension
    {
        public static async void Forget(this Awaitable awaitable, bool logException = true)
        {
            try
            {
                await awaitable;
            }
            catch (Exception e)
            {
                if (logException) Debug.LogException(e);
            }
        }
        
        public static async Awaitable SuppressCancellationThrow(this Awaitable awaitable)
        {
            try
            {
                await awaitable;
            }
            catch (OperationCanceledException)
            {
            }
        }
    }
}