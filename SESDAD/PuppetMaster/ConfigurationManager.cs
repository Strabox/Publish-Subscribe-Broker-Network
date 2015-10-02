using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PuppetMaster
{
    public static class ConfigurationManager
    {

        public static string CONFIG_FILES_DIRECTORY = "..\\..\\ConfigurationFiles\\";


        public static void ReadConfigurationFile(string filePath)
        {
            string[] tokens = null, lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                if (Regex.IsMatch(line,ParseUtil.SITE))
                {
                    tokens = Regex.Split(line, ParseUtil.SPACE);
                    Debug.WriteLine("[Broker Tree]Name: {0} Parent: {1}",
                        tokens[1],tokens[3]);
                }
                else if (Regex.IsMatch(line,ParseUtil.PROCESS))
                {
                    tokens = Regex.Split(line, ParseUtil.SPACE);
                    Debug.WriteLine("[Process]Name {0} Type: {1} Where {2} URL: {3}",
                        tokens[1],tokens[3],tokens[5],tokens[7]);
                }
                else if (Regex.IsMatch(line,ParseUtil.ROUTING))
                {
                    tokens = Regex.Split(line, ParseUtil.SPACE);
                    Debug.WriteLine(tokens[1]);
                }
                else if (Regex.IsMatch(line,ParseUtil.ORDERING))
                {
                    tokens = Regex.Split(line, ParseUtil.SPACE);
                    Debug.WriteLine(tokens[1]);
                }
            }
        }

    }
}
