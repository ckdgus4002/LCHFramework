using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LCHFramework.Attributes;
using UnityEditor;

namespace LCHFramework.Editor.Utilities
{
    public class TypeUtility
    {
        private static readonly Dictionary<Type, List<MethodInfo>> _methodInfos = new();
        public static List<MethodInfo> GetMethodInfos(Type targetType)
        {
            if (!_methodInfos.ContainsKey(targetType))
                _methodInfos[targetType] = TypeCache.GetMethodsWithAttribute<ButtonAttribute>()
                    .Where(r => r.DeclaringType!.IsAssignableFrom(targetType)
                                || (targetType.BaseType!.IsGenericType && targetType.BaseType.GetGenericTypeDefinition() == r.DeclaringType))
                    .OrderBy(r => r.MetadataToken)
                    .ToList();
            
            return _methodInfos[targetType];
        }
    }
}