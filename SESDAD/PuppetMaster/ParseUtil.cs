using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PuppetMaster
{
    public static class ParseUtil
    {
        public static string SITE = "^Site";
        public static string ORDERING = "^Ordering";
        public static string ROUTING = "^RoutingPolicy";
        public static string PROCESS = "^Process";
        public static string LOGGING_LEVEL = "^LoggingLevel";
        public static string SPACE = "[ \t]+";

        public static int ExtractPortFromURL(string url)
        {
            return new Uri(url).Port;
        }

        public static string ExtractPorFromURLS(string url)
        {
            return new Uri(url).Port.ToString();
        }

        public static string ExtractIPFromURL(string url)
        {
            return new Uri(url).Host;
        }

    }
}
