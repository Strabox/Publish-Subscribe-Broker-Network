using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace CommonTypes
{
    /// <summary>
    /// Contains useful methods and constants to system nodes.
    /// </summary>
    public class Util
    {
        public static string PROJECT_ROOT = ".." +
                            Path.DirectorySeparatorChar + ".." +
                            Path.DirectorySeparatorChar + ".." +
                            Path.DirectorySeparatorChar;
        public static string EXE_PATH = Path.DirectorySeparatorChar +
                            "bin" + Path.DirectorySeparatorChar +
                            "Debug" + Path.DirectorySeparatorChar;

        public static string ROOT = "Root";

        public static string PUPPET_MASTER_NAME = "PuppetMasterURL";

        public static int PUPPET_MASTER_PORT = 30000;

        public static bool IsLinux
        {
            get
            {
                int p = (int)Environment.OSVersion.Platform;
                return (p == 4) || (p == 6) || (p == 128);
            }
        }

        public static string ExtractPath(string uri)
        {
            return new Uri(uri).LocalPath.Substring(1);
        }

        public static string MakeUrl(string protocol,string ip, string port,
            string path)
        {
            return protocol + "://" + ip + ":" + port + "/" + path;
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return null;
        }

    }
}
