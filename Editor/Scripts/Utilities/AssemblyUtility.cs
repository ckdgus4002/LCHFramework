using System;
using System.Reflection;

namespace LCHFramework.Editor.Utilities
{
    public static class AssemblyUtility
    {
        public static object InvokeMethod(string typeName, string methodName, BindingFlags methodBindingAttr)
            => InvokeMethod(typeName, methodName, methodBindingAttr, null);
        
        public static object InvokeMethod(string typeName, string methodName, BindingFlags methodBindingAttr, object invokeObj)
            => InvokeMethod(typeName, methodName, methodBindingAttr, invokeObj, new object[] { });
        
        public static object InvokeMethod(string typeName, string methodName, BindingFlags methodBindingAttr, object invokeObj, object[] invokeParameters)
        {
            object resultOrNull = null;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var typeOrNull = assembly.GetType(typeName); 
                if (typeOrNull != null) resultOrNull = typeOrNull.GetMethod(methodName, methodBindingAttr)!.Invoke(invokeObj, invokeParameters);
            }
            
            return resultOrNull;
        }
    }
}
