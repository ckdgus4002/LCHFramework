using System;

namespace LCHFramework.Utilities
{
    public static class IDisposableUtility
    {
        public static void DisposeAndSetNull<T>(ref T disposable) where T : class, IDisposable
        {
            disposable?.Dispose();
            disposable = null;
        }
    }
}