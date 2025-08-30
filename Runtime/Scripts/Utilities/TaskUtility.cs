using System;
using System.Threading.Tasks;

namespace LCHFramework.Utilities
{
    public static class TaskUtility
    {
        public static async Task WaitUntil(Func<bool> predicate) { while (!predicate()) await Task.Yield(); }
        
        public static async Task WaitWhile(Func<bool> predicate) { while (predicate()) await Task.Yield(); }
        
        
        
        public static class CompletedTasks
        {
            public static Task<bool> False => new(() => false);
        
            public static Task<bool> True => new(() => true);    
        }
    }
}