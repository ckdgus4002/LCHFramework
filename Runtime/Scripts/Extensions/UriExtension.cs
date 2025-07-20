using System;
using System.Collections.Generic;
using System.Linq;

namespace LCHFramework.Extensions
{
    public static class UriExtension
    {
        public static Uri SetQuery(this Uri uri, Dictionary<string, string> query)
            => query.Aggregate(uri, (aggregate, t) => aggregate.SetQuery(t.Key, t.Value));
        
        public static Uri SetQuery(this Uri uri, string name, string value)
            => new($"{uri.OriginalString}{(!uri.OriginalString.Contains("?") ? "?" : "&")}{name}={value}");
    }
}