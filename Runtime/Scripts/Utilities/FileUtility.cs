namespace LCHFramework.Utilities
{
    public enum FileUnit { B, KB, MB, GB, TB, PB, EB, }

    public static class FileUtility
    {
        public static string ToHumanReadableFileSize(long @byte, int? decimalNumberOrNull = null)
        {
            var (fileSize, fileUnit) = _ToHumanReadableFileSize(@byte);
            decimalNumberOrNull ??= (int)fileUnit;
            return $"{fileSize.ToString($"f{decimalNumberOrNull}")} {fileUnit}";
        }
    
        private static (float, FileUnit) _ToHumanReadableFileSize(long @byte)
        {
            var fileSize = (float)@byte;
            var fileUnit = FileUnit.B;
            while (1024 <= fileSize)
            {
                fileSize /= 1024;
                fileUnit++;
            }
            return (fileSize, fileUnit);
        }
    }

}