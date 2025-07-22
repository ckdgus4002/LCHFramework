using System.Collections.Generic;

namespace LCHFramework.Data
{
    public static class ServerAPIData
    {
        public static class ContentType
        {
            public static KeyValuePair<string, string> All = new("Content-Type", "*/*");
            public static KeyValuePair<string, string> ApplicationJson = new("Content-Type", "application/json");
            public static KeyValuePair<string, string> AudioWav = new("Content-Type", "audio/wav");
            public static KeyValuePair<string, string> ImageJpeg = new("Content-Type", "image/jpeg");
            public static KeyValuePair<string, string> TextPlain = new("Content-Type", "text/plain");
            public static KeyValuePair<string, string> VideoMp4 = new("Content-Type", "video/mp4");
        }
    }
}