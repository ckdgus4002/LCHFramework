using System.Text;

namespace LCHFramework.Utils
{
    public static class FileUtil
    {
        public static string ToHumanReadableFileSize(long @byte, int decimalNumber)
        {
            var result = (float)@byte;
            var i = 0;
            while (1024 <= result)
            {
                result /= 1024;
                i++;
            }

            var stringBuilder = new StringBuilder();
            stringBuilder.Append(result.ToString($"f{decimalNumber}"));
            stringBuilder.Append(" ");
            stringBuilder.Append(i switch { 0 => "B", 1 => "KB", 2 => "MB", 3 => "GB", 4 => "TB", 5 => "PB", 6 => "EB", _ => string.Empty });
            return stringBuilder.ToString();
        }
    }    
}