using CommonTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuppetMaster
{
    public class LogServer : MarshalByRefObject, IPuppetMasterLog
    {
        public static string LOG_FILES_DIRECTORY = ".."
                             + Path.DirectorySeparatorChar +
                             ".." + Path.DirectorySeparatorChar +
                             "LogFiles" + Path.DirectorySeparatorChar;

        private string logFile = null;

        public string LogFile
        {
            get
            {
                return logFile;
            }
            set
            {
                logFile = value;
            }
        }

        public void LogAction(string logMessage)
        {
            lock (this)
            {
                if (logFile == null)
                    return;
                StreamWriter writer = File.AppendText(
                LOG_FILES_DIRECTORY + logFile);
                writer.Write(logMessage + Environment.NewLine);
                writer.Close();
            }
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
