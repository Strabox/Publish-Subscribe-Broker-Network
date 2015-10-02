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
        public static string IS = "Is";
        public static string ON = "On";
        public static string URL = "URL";
        public static string PARENT = "Parent";
        public static string PROCESS_TYPE = "(Publisher|Subscriber|Broker)";
        public static string SPACE = "[ \t]+";
        public static string NAME = "([0-9]|[A-Z]|[a-z])+";

    }
}
