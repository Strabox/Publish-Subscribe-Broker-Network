using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuppetMaster
{
    public static class ConfigurationManager
    {

        public static string CONFIG_FILES_DIRECTORY = "..\\..\\ConfigurationFiles\\";


        public static void ReadConfigurationFile(string filePath)
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
                Debug.WriteLine(line);
            //TODO parse and launch all processes needed.
        }

    }
}
