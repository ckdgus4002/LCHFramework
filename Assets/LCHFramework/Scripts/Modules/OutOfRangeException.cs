using System;

namespace LCHFramework.Modules
{
    public class OutOfRangeException : Exception
    {
        public OutOfRangeException()
        {
        }
        
        public OutOfRangeException(string message) : base(message)
        {
        }
    }
}