namespace LCHFramework.Extensions
{
    public static class StringExtension
    {
        public static int[] SplitInts(this string str, char separator)
        {
            var split = str.Split(separator);
            var array = new int[split.Length];
            for (var i = 0; i < array.Length; i++) array[i] = int.Parse(split[i]);
            return array;
        }
    }
}