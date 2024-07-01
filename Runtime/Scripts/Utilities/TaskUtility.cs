using System.Threading.Tasks;

namespace LCHFramework.Utilities
{
    public static class TaskUtility
    {
        public static class CompletedTasks
        {
            public static Task<bool> False => new(() => false);
        
            public static Task<bool> True => new(() => true);    
        }
    }
}