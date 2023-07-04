namespace LCHFramework.Utils
{
    public static class StringUtils
    {
        public static float Compare(string strA, string strB)
        {
            return strA != strB ? 0 : 1;
        }
    }
}