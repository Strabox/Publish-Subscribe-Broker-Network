using CommonTypes;
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
    public class ConfigurationManager
    {
        private static string LOG_CRASHED = "Crash";
        public static string CONFIG_FILES_DIRECTORY = ".."+
                            Path.DirectorySeparatorChar+
                            ".."+ Path.DirectorySeparatorChar+
                            "ConfigurationFiles" + Path.DirectorySeparatorChar;
        public static string LOG_FILES_DIRECTORY = ".."
                             + Path.DirectorySeparatorChar +
                             ".."+ Path.DirectorySeparatorChar +
                            "LogFiles"+ Path.DirectorySeparatorChar;
        private static string PROJECT_ROOT = ".." + 
                            Path.DirectorySeparatorChar + ".." + 
                            Path.DirectorySeparatorChar + ".." + 
                            Path.DirectorySeparatorChar;
        private static string EXE_PATH = Path.DirectorySeparatorChar+
                            "bin"+ Path.DirectorySeparatorChar + 
                            "Debug"+ Path.DirectorySeparatorChar;

        // Pair <ProcessName,URL>
        private Dictionary<string, string> processes;

        private FormPuppetMaster parentForm;

        private string currentConfigurationRunning;

        public ConfigurationManager(FormPuppetMaster form)
        {
            processes = new Dictionary<string, string>();
            this.parentForm = form;
        }

        // Very AD HOC method :) - Parse the URL to get Port and Name
        // So ad hoc that only works with ports with 4 digits :)
        private string[] ParseURL(string url)
        {
            //res[0] = port, res[1] = name
            string[] res = new string[2];
            res[0] = url.Substring(16, 4);
            res[1] = url.Substring(21, url.Length - 21);
            return res;
        }


        public void LogSystemActions(string logAction)
        {
            StreamWriter writer = File.AppendText(
                LOG_FILES_DIRECTORY + currentConfigurationRunning);
            writer.Write(logAction + Environment.NewLine);
            writer.Flush();
            writer.Close();
        }

        public void ReadConfigurationFile(string filePath)
        {
            string[] tokens = null, lines = File.ReadAllLines(filePath);
            currentConfigurationRunning = Path.GetFileName(filePath);
            File.Create(LOG_FILES_DIRECTORY + currentConfigurationRunning).Close();
            parentForm.ReloadLogFiles();
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
                    string processType = tokens[3].First().ToString().ToUpper() 
                          + tokens[3].Substring(1);
                    string[] urlParse = ParseURL(tokens[7]);
                    processes.Add(tokens[1], tokens[7]);
                    parentForm.Invoke(new AddProcess(parentForm.AddToGenericProcesses),
                        tokens[1]);
                    if (processType.Equals("Publisher"))
                        parentForm.Invoke(new AddProcess(parentForm.AddToPublishersProcesses),
                            tokens[1]);
                    else if (processType.Equals("Subscriber"))
                        parentForm.Invoke(new AddProcess(parentForm.AddToSubscribersProcesses),
                            tokens[1]);
                    Process.Start(PROJECT_ROOT + processType + EXE_PATH + processType,
                        String.Join(" ",urlParse));
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

        /* ########################## Remote Calls ########################## */

        public void Status()
        {
            //TODO
        }

        public void CrashAll()
        {
            foreach(KeyValuePair<string,string> pair in processes)
            {
                ICommon node = Activator.GetObject(typeof(ICommon),pair.Value) as ICommon;
                node.Crash();
                LogSystemActions(string.Join(" ", LOG_CRASHED,pair.Key));
            }
            processes = new Dictionary<string, string>();
        }

        public void Crash(string processName)
        {
            if (!processes.ContainsKey(processName))
                return;
            ICommon node = Activator.GetObject(typeof(ICommon), processes[processName])
                as ICommon;
            node.Crash();
            LogSystemActions(string.Join(" ",LOG_CRASHED,processName));
            processes.Remove(processName);
        }

        public void Freeze(string processName)
        {
            //TODO
        }

        public void Unfreeze(string processName)
        {
            //TODO
        }

        public void Subscribe(string processName,string topic)
        {
            //TODO
        }

        public void Unsubscribe(string processName,string topic)
        {
            //TODO
        }

    }
}
