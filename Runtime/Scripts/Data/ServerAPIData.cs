using System.Collections.Generic;

namespace LCHFramework.Data
{
    public static class ServerAPIData
    {
        public static class ContentType
        {
            public static KeyValuePair<string, string> ApplicationJson = new("Content-Type", "application/json");
        }
    }
}