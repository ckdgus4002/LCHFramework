using UnityEngine;

namespace LCHFramework.Packages.LCHFramework.Runtime.Scripts.Extensions
{
    public static class AwaitableExtension
    {
        public static async Awaitable Forget(this Awaitable awaitable)
        {
            try
            {
                await awaitable;
            }
            catch
            {
                // ignored
            }
        }
    }

}