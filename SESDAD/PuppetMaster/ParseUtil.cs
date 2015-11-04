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
        public static string NAME = "([0-9]|[A-Z]|[a-z]|\\-)+";
        public static string SPACE = "[ \t]+";
        public static string TOPIC_NAME = "([0-9]|[A-Z]|[a-z]|\\*|\\-|/)+"; //TODO correct it :)

        //Configuration File
        public static string SITE = "^Site" + SPACE + NAME + SPACE +  "Parent" + SPACE + NAME;
        public static string ORDERING = "^Ordering"+ SPACE + "(NO|FIFO|TOTAL)";
        public static string ROUTING = "^RoutingPolicy"+ SPACE + "(flooding|filter)";
        public static string LOGGING_LEVEL = "^LoggingLevel" + SPACE + "(light|full)";
        public static string PROCESS = "^Process" + SPACE + NAME + SPACE + "Is" + SPACE +
            "(broker|publisher|subscriber)" + SPACE + "On" + SPACE + NAME + SPACE
            + "URL";    //TODO unfinished match for URL

        //Script files
        public static string CRASH = "^Crash" + SPACE + NAME;
        public static string STATUS = "^Status";
        public static string FREEZE = "^Freeze" + SPACE + NAME;
        public static string WAIT = "^Wait" + SPACE + "[0-9]+";
        public static string UNFREEZE = "^Unfreeze" + SPACE + NAME;
        public static string PUBLISH = "^Publisher" + SPACE + NAME + SPACE + "Publish"
            + SPACE + "[0-9]+" + SPACE + "Ontopic" + SPACE + TOPIC_NAME + SPACE + "Interval" 
            + SPACE + "[0-9]+";
        public static string SUBSCRIBE = "^Subscriber" + SPACE + NAME + SPACE 
            + "Subscribe" + SPACE + TOPIC_NAME;
        public static string UNSUBSCRIBE = "^Subscriber" + SPACE + NAME + SPACE
            + "Unsubscribe" + SPACE + TOPIC_NAME;

        public static string ExtractPortFromURL(string url)
        {
            return new Uri(url).Port.ToString();
        }

        public static string ExtractIPFromURL(string url)
        {
            return new Uri(url).Host;
        }

    }
}
