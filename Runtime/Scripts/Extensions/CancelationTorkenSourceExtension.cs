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
                const int index = 0;
                cancellationTokenSources.RemoveAt(index);
                var cts = cancellationTokenSources[index];
                ClearTokenSource(ref cts);
            }
        }
        
        public static void ClearTokenSource(ref CancellationTokenSource cancellationTokenSource)
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
        }
    }
}
