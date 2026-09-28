namespace LCHFramework.Editor.Utilities
{
    public static class ArrayUtility
    {
        public static T AddAndReturnItem<T>(ref T[] array, T item)
        {
            UnityEditor.ArrayUtility.Add(ref array, item);
            return item;
        }
    }
}