using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace LCHFramework.Editor.Utilities
{
    public class TypeUtility<T> where T : Attribute
    {
        private static readonly Dictionary<Type, List<MethodInfo>> _methodInfos = new();
        public static List<MethodInfo> GetMethodInfos(Type type)
        {
            if (!_methodInfos.ContainsKey(type))
                _methodInfos[type] = TypeCache.GetMethodsWithAttribute<T>()
                    .Where(r => r.DeclaringType!.IsAssignableFrom(type)
                                || (type.BaseType!.IsGenericType && type.BaseType.GetGenericTypeDefinition() == r.DeclaringType))
                    .OrderBy(r => r.MetadataToken)
                    .ToList();
            
            return _methodInfos[type];
        }
    }
}