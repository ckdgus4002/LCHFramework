using System;
using System.Linq;
using UnityEngine;

namespace LCHFramework.Packages.LCHFramework.Runtime.Scripts.Extensions
{
    public static class AwaitableExtension
    {
        public static async Awaitable Forget(this Awaitable awaitable) => await Forget(awaitable, new OperationCanceledException());
        
        public static async Awaitable Forget(this Awaitable awaitable, params Exception[] ignoreExceptions)
        {
            try
            {
                await awaitable;
            }
            catch (Exception e)
            {
                if (ignoreExceptions.All(t => t.GetType() != e.GetType()))
                    throw;
            }
        }
    }
}