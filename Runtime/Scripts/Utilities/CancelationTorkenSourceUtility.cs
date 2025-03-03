using System.Collections.Generic;
using System.Threading;
using LCHFramework.Extensions;

namespace LCHFramework.Utilities
{
    public static class CancellationTokenSourceUtility
    {
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
            if (cancellationTokenSource.Token.CanBeCanceled) cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
        }
    }
}
