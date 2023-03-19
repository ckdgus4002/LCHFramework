using System;
using System.Linq;

namespace LCHFramework.Extensions
{
    public static class DelegateExtension
    {
        public static bool Contains(this Delegate @delegate, Delegate value) => @delegate != null && @delegate.GetInvocationList().Contains(value);
    }
}