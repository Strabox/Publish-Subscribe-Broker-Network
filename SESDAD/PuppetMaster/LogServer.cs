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

        private LogManager logManager;

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

        public LogServer()
        {
            logManager = new LogManager();
        }

        public void LogAction(string logMessage)
        {
            logManager.LogSystemAction(logMessage,LogFile);
        }

        /*As far as I know this is necessary to override the
       .NET lease times. */
        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
