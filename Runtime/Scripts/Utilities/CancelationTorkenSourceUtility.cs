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
            while (!cancellationTokenSources.IsEmpty())
            {
                var cts = cancellationTokenSources[0];
                ClearTokenSource(ref cts);
                cancellationTokenSources.RemoveAt(0);
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
