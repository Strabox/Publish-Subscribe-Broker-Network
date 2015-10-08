using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        public static string NAME = "([0-9]|[A-Z]|[a-z])+";

        // Very AD HOC method :) - Parse the URL to get Port and Name
        // So ad hoc that only works with ports with 4 digits :)
        public static string[] ParseURL(string url)
        {
            //res[0] = port, res[1] = name
            string[] res = new string[2];
            res[0] = url.Substring(16, 4);
            res[1] = url.Substring(21, url.Length - 21);
            return res;
        }

    }
}
