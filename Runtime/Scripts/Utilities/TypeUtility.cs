using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LCHFramework.Utilities
{
    public class TypeUtility
    {
        public const BindingFlags MaxBindingFlags = (BindingFlags)62;
        
        private static readonly Dictionary<Type, Dictionary<int, MethodInfo>> _methodInfos = new();
        public static MethodInfo GetMethodInfo(Type type, string methodName, params Type[] argumentTypes)
        {
            var methodHash = methodName.GetHashCode() ^ argumentTypes.Aggregate(0, (hash, r) => hash ^ r.GetHashCode());
            if (_methodInfos.TryGetValue(type, out var methodInfosByHashes))
                if (methodInfosByHashes.TryGetValue(methodHash, out var methodInfo))
                    return methodInfo;
            
            if (!_methodInfos.ContainsKey(type))
                _methodInfos[type] = new Dictionary<int, MethodInfo>();
            
            for (var curType = type; curType != null; curType = curType.BaseType)
                if (curType.GetMethod(methodName, MaxBindingFlags, null, argumentTypes, null) is { } methodInfo)
                    return _methodInfos[type][methodHash] = methodInfo;

            foreach (var interfaceType in type.GetInterfaces())
                if (interfaceType.GetMethod(methodName, MaxBindingFlags, null, argumentTypes, null) is { } methodInfo)
                    return _methodInfos[type][methodHash] = methodInfo;

            return _methodInfos[type][methodHash] = null;
        }
    }
}