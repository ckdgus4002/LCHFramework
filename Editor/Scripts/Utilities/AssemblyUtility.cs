using System.Reflection;
#if UNITY_6000_6_OR_NEWER
using UnityEngine.Assemblies;
#else
using System;
#endif

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
#if UNITY_6000_6_OR_NEWER
            foreach (var assembly in CurrentAssemblies.GetLoadedAssemblies())
#else
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
#endif
            {
                var typeOrNull = assembly.GetType(typeName); 
                if (typeOrNull != null) resultOrNull = typeOrNull.GetMethod(methodName, methodBindingAttr)!.Invoke(invokeObj, invokeParameters);
            }
            
            return resultOrNull;
        }
    }
}
