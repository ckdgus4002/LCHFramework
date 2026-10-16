using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LCHFramework.Extensions
{
    public static class StringExtension
    {
        private static readonly Regex splitWordsRegex = new(@"[\s_-]+|(?<=[a-z0-9])(?=[A-Z])|(?<=[A-Z])(?=[A-Z][a-z])");
        private static IEnumerable<string> SplitWords(string str) => splitWordsRegex.Split(str).Where(word => 0 < word.Length);
        
        public static string ToPascal(this string str, string separator = "") => string.IsNullOrEmpty(str) ? string.Empty : string.Join(separator, SplitWords(str).Select(word => word.ToCapitalize()));
        
        public static string ToCamel(this string str, string separator = "") => string.IsNullOrEmpty(str) ? string.Empty : string.Join(separator, SplitWords(str).Select((word, index) => index == 0 ? word.ToLowerInvariant() : word.ToCapitalize()));
        
        public static string ToCapitalize(this string str, char repetition = char.MinValue)
        {
            if (string.IsNullOrEmpty(str)) return string.Empty;
            
            var stringBuilder = new StringBuilder(str.Length);
            for (var i = 0; i < str.Length; i++)
                stringBuilder.Append(i == 0 || str[i - 1] == repetition ? char.ToUpperInvariant(str[i]) : char.ToLowerInvariant(str[i]));
            
            return stringBuilder.ToString();
        }
    }
}