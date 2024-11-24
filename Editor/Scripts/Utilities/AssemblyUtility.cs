using System;
using System.Reflection;

namespace LCHFramework.Editor.Utilities
{
    public static class AssemblyUtility
    {
        public static object InvokeMethod(string typeName, string methodName, BindingFlags methodBindingAttr, object invokeObj = null, object[] invokeParameters = null)
        {
            object resultOrNull = null;
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var typeOrNull = assembly.GetType(typeName); 
                if (typeOrNull == null) continue;
                
                resultOrNull = typeOrNull.GetMethod(methodName, methodBindingAttr)!.Invoke(invokeObj, invokeParameters);
            }

            return resultOrNull;
        }
    }
}
