using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuppetMaster
{
    public class LogManager
    {

        public static string LOG_FILES_DIRECTORY = ".."
                             + Path.DirectorySeparatorChar +
                             ".." + Path.DirectorySeparatorChar +
                             "LogFiles" + Path.DirectorySeparatorChar;


        public static void CreateLogFile(string logFileName)
        {
            File.CreateText(LOG_FILES_DIRECTORY + logFileName).Close();
        }

        public void LogSystemAction(string logAction,string logFile)
        {
            if (logFile == null)
                return;
            StreamWriter writer = File.AppendText(
            LOG_FILES_DIRECTORY + logFile);
            writer.Write(logAction + Environment.NewLine);
            writer.Flush();
            writer.Close();
        }

    }
}
