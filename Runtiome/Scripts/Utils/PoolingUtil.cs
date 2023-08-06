using System;
using System.Collections.Generic;
using UnityEngine;

namespace LCHFramework.Utils
{
    public static class PoolingUtil
    {
        public static void Pooling<T>(int itemCount, ref List<T> list, Action actionOnStart, Func<T> createFunc, Action<int, T> actionOnAdd, Action<T> actionOnRemove)
        {
            actionOnStart?.Invoke();
            for (var i = 0; i < Mathf.Max(itemCount, list.Count);)
            {
                if (i < itemCount)
                {
                    var item = list.Count <= i ? createFunc.Invoke() : list[i];
                    if (!list.Contains(item)) list.Add(item);
                    actionOnAdd?.Invoke(i, item);
                    ++i;
                }
                else
                {
                    var item = list[i];
                    list.RemoveAt(i);
                    actionOnRemove?.Invoke(item);
                }
            }
        }
    }
}