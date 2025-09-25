using System.Text;
using System.Text.RegularExpressions;

namespace LCHFramework.Extensions
{
    public static class StringExtension
    {
        public static string ToCapitalize(this string str, char repetition = char.MinValue)
        {
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < str.Length; i++)
                stringBuilder.Append(i == 0 || str[i - 1] == repetition ? char.ToUpper(str[i]) : char.ToLower(str[i]));

            return stringBuilder.ToString();
        }
        
        public static string ToPascal(this string str)
            => string.IsNullOrEmpty(str) ? "" : Regex.Replace(str, "(?<!^)([A-Z])", " $1");
    }
}