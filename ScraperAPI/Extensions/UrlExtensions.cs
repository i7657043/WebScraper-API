using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScraperAPI.Extensions
{
    public static class UrlExtensions
    {
        public static string RemoveTrailingSlash(this string url)
        {
            if (url.Substring(url.Length - 1, 1) == "/")
                return url.Remove(url.Length - 1, 1);

            return url;
        }
    }
}
