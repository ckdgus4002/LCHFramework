using System;

namespace LCHFramework.Data
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