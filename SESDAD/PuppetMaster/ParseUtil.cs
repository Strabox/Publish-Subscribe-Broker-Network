using CommonTypes;
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
        public static string NAME = "([0-9]|[A-Z]|[a-z])+";
        public static string SPACE = "[ \t]+";

        //Configuration File
        public static string SITE = "^Site" + SPACE + NAME + SPACE +  "Parent" + SPACE + NAME;
        public static string ORDERING = "^Ordering"+ SPACE + "(NO|FIFO|TOTAL)";
        public static string ROUTING = "^RoutingPolicy"+ SPACE + "(flooding|filter)";
        public static string LOGGING_LEVEL = "^LoggingLevel" + SPACE + "(light|full)";
        public static string PROCESS = "^Process" + SPACE + NAME + SPACE + "Is" + SPACE +
            "(broker|publisher|subscriber)" + SPACE + "On" + SPACE + NAME + SPACE
            + "URL";

        //Script files
        public static string CRASH = "^Crash" + SPACE + NAME;
        public static string STATUS = "^Status";
        public static string FREEZE = "^Freeze" + SPACE + NAME;
        public static string WAIT = "^Wait" + SPACE + "[0-9]+";
        public static string UNFREEZE = "^Unfreeze" + SPACE + NAME;
        public static string PUBLISH = "^PubEvent";
        public static string SUBSCRIBE = "^SubEvent";

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
