namespace LCHFramework
{
    public static class Array
    {
        public static int[] NewSequential(int startInclusive, int spacing, int length)
        {
            var array = new int[length];
            for (var i = 0; i < length; i++) array[i] = startInclusive + spacing * i;

            return array;
        }
    }
}