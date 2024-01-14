using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCHFramework.Utils
{
    public static class PoolingUtil
    {
        public static void Pooling<T>(int itemCount, ref List<T> list, Func<T> createFunc, Action<int, T> actionOnAddOrNull = null, Action<T> actionOnRemoveOrNull = null)
        {
            for (var i = 0; i < Mathf.Max(itemCount, list.Count);)
            {
                if (i < itemCount)
                {
                    var item = list.Count <= i ? createFunc.Invoke() : list[i];
                    if (!list.Contains(item)) list.Add(item);
                    actionOnAddOrNull?.Invoke(i, item);
                    ++i;
                }
                else
                {
                    var item = list[i];
                    list.Remove(item);
                    actionOnRemoveOrNull?.Invoke(item);
                }
            }
        }
    }
}