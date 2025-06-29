using System.Collections.Generic;
using System.Threading;
using LCHFramework.Extensions;

namespace LCHFramework.Utilities
{
    public static class CancellationTokenSourceUtility
    {
        public static void RestartTokenSources(ref CancellationTokenSource cancellationTokenSource)
        {
            ClearTokenSource(ref cancellationTokenSource);
            cancellationTokenSource = new CancellationTokenSource();
        }
        
        public static void ClearTokenSources(List<CancellationTokenSource> cancellationTokenSources)
        {
            while (cancellationTokenSources is { Count: > 0 })
            {
                var index = cancellationTokenSources.Count - 1;
                var cts = cancellationTokenSources[index];
                ClearTokenSource(ref cts);
                cancellationTokenSources.RemoveAt(index);
            }
        }
        
        public static void ClearTokenSource(ref CancellationTokenSource cancellationTokenSource)
        {
            if (cancellationTokenSource == null) return;
            
            if (cancellationTokenSource.Token.CanBeCanceled) cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
        }
    }
}
