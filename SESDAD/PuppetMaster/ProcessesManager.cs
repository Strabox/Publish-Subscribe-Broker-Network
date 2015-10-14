using CommonTypes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace PuppetMaster
{
    public class ProcessesManager
    {

        public static string CONFIG_FILES_DIRECTORY = ".."+
                            Path.DirectorySeparatorChar+
                            ".."+ Path.DirectorySeparatorChar+
                            "ConfigurationFiles" + Path.DirectorySeparatorChar;
        public static string SCRIPT_FILES_DIRECTORY = ".." +
                            Path.DirectorySeparatorChar +
                            ".." + Path.DirectorySeparatorChar +
                            "ScriptFiles" + Path.DirectorySeparatorChar;
        public static string LOG_FILES_DIRECTORY = ".."
                             + Path.DirectorySeparatorChar +
                             ".." + Path.DirectorySeparatorChar +
                             "LogFiles" + Path.DirectorySeparatorChar;

        private FormPuppetMaster parentForm;

        private LogServer logServer;

        // Pair <ProcessName,URL>
        private Dictionary<string, string> processes;


        public ProcessesManager(FormPuppetMaster form)
        {
            processes = new Dictionary<string, string>();
            logServer = new LogServer();
            RemotingServices.Marshal(logServer, CommonConstants.PUPPET_MASTER_NAME,
                typeof(LogServer));
            this.parentForm = form;
        }

        public void ReadConfigurationFile(string filePath)
        {
            ManageSites sites = new ManageSites();
            ProcessLauncher launcher = new ProcessLauncher();
            string[] tokens = null, lines = File.ReadAllLines(filePath);
            logServer.LogFile = "Log" + Path.GetFileName(filePath);
            File.CreateText(LOG_FILES_DIRECTORY + logServer.LogFile).Close(); 
            parentForm.ReloadLogFiles();
            foreach (string line in lines)
            {
                if (Regex.IsMatch(line, ParseUtil.SITE))
                {
                    tokens = Regex.Split(line, ParseUtil.SPACE);
                    Site children = sites.CreateSite(tokens[1]);
                    if (!tokens[3].Equals("none"))
                    {
                        Site parent = sites.CreateSite(tokens[3]);
                        children.Parent = parent;
                        parent.AddChildrenSite(children);
                    }
                    else
                        children.Parent = null;
                }
                else if (Regex.IsMatch(line, ParseUtil.PROCESS))
                {
                    tokens = Regex.Split(line, ParseUtil.SPACE);
                    string processType = tokens[3];
                    string port = ParseUtil.ExtractPorFromURLS(tokens[7]);
                    processes.Add(tokens[1], tokens[7]);
                    parentForm.Invoke(new AddProcess(parentForm.AddToGenericProcesses),
                        tokens[1]);
                    if (processType.Equals("publisher"))
                    {
                        parentForm.Invoke(new AddProcess(parentForm.AddToPublishersProcesses),
                            tokens[1]);
                        launcher.AddNode(new LaunchPublisher(tokens[1],
                            ParseUtil.ExtractIPFromURL(tokens[7]), port, tokens[5]));
                    }
                    else if (processType.Equals("subscriber"))
                    {
                        parentForm.Invoke(new AddProcess(parentForm.AddToSubscribersProcesses),
                            tokens[1]);
                        launcher.AddNode(new LaunchSubscriber(tokens[1],
                            ParseUtil.ExtractIPFromURL(tokens[7]), port, tokens[5]));
                    }
                    else if (processType.Equals("broker"))
                    {
                        sites.GetSiteByName(tokens[5]).AddBrokerUrlToSite(tokens[7]);
                        launcher.AddNode(new LaunchBroker(tokens[1],
                           ParseUtil.ExtractIPFromURL(tokens[7]), port, tokens[5]));
                    }
                }
                else if (Regex.IsMatch(line, ParseUtil.ROUTING))
                {
                    tokens = Regex.Split(line, ParseUtil.SPACE);
                    launcher.RoutingPolicy = tokens[1];
                }
                else if (Regex.IsMatch(line, ParseUtil.ORDERING))
                {
                    tokens = Regex.Split(line, ParseUtil.SPACE);
                    launcher.OrderingPolicy = tokens[1];
                }
                else if (Regex.IsMatch(line, ParseUtil.LOGGING_LEVEL))
                {
                    tokens = Regex.Split(line, ParseUtil.SPACE);
                    launcher.LogPolicy = tokens[1];
                }
            }
            launcher.LaunchAllProcesses(sites); 
        }

        public void ReadScriptFile(string scriptFilePath)
        {
            string[] lines = File.ReadAllLines(scriptFilePath);
            foreach (string line in lines)
            {
                if (Regex.IsMatch(line, "^Publisher"))
                {
                    string[] tokens = line.Split(' ');
                    Publish(tokens[1], int.Parse(tokens[7]), int.Parse(tokens[3]),
                        tokens[5]);
                }
                else if (Regex.IsMatch(line, "^Subscriber"))
                {
                    string[] tokens = line.Split(' ');
                    Subscribe(tokens[1], tokens[3]);
                }
                else if (Regex.IsMatch(line, "^Crash"))
                {
                    string[] tokens = line.Split(' ');
                    Crash(tokens[1]);
                }
                else if (Regex.IsMatch(line, "^Freeze"))
                {
                    string[] tokens = line.Split(' ');
                    Freeze(tokens[1]);
                }
                else if (Regex.IsMatch(line, "^Unfreeze"))
                {
                    string[] tokens = line.Split(' ');
                    Unfreeze(tokens[1]);
                }
                else if (Regex.IsMatch(line, "^Status"))
                {
                    Status();
                }
                else if (Regex.IsMatch(line, "^Wait"))
                {
                    string[] tokens = line.Split(' ');
                    Thread.Sleep(int.Parse(tokens[1]));
                }
            }
        }
        /* ########################## Remote Calls ########################## */

        public void Status()
        {
            foreach(KeyValuePair<string,string> pair in processes)
            {
                ICommon node = Activator.GetObject(typeof(ICommon), pair.Value)
                    as ICommon;
                node.Status();
            }
            logServer.LogAction("Status");
        }

        public void CrashAll()
        {
            foreach(KeyValuePair<string,string> pair in processes)
            {
                ICommon node = Activator.GetObject(typeof(ICommon),pair.Value)
                    as ICommon;
                node.Crash();
                logServer.LogAction("Crash " + pair.Key);
            }
            processes.Clear();
            parentForm.BeginInvoke(new Enable(parentForm.EnableConfigFiles), true);
        }

        public void Crash(string processName)
        {
            if (!processes.ContainsKey(processName))
                return;
            ICommon node = Activator.GetObject(typeof(ICommon), 
                processes[processName]) as ICommon;
            node.Crash();
            logServer.LogAction("Crash " + processName);
            processes.Remove(processName);
            if(processes.Count == 0)
                parentForm.BeginInvoke(new Enable(parentForm.EnableConfigFiles), true);
        }

        public void Freeze(string processName)
        {
            if (!processes.ContainsKey(processName))
                return;
            ICommon node = Activator.GetObject(typeof(ICommon), 
                processes[processName]) as ICommon;
            node.Freeze();
            logServer.LogAction("Freeze " + processName);
        }

        public void Unfreeze(string processName)
        {
            if (!processes.ContainsKey(processName))
                return;
            ICommon node = Activator.GetObject(typeof(ICommon),
                processes[processName]) as ICommon;
            node.Unfreeze();
            logServer.LogAction("Unfreeze " + processName);
        }

        public void Subscribe(string processName,string topic)
        {
            if (!processes.ContainsKey(processName))
                return;
            ISubscriber node = Activator.GetObject(typeof(ISubscriber),
                processes[processName]) as ISubscriber;
            node.Subscribe(topic);
            logServer.LogAction("Subscriber " + processName + " Subscribe " + topic);
        }

        public void Unsubscribe(string processName,string topic)
        {
            if (!processes.ContainsKey(processName))
                return;
            ISubscriber node = Activator.GetObject(typeof(ISubscriber),
                processes[processName]) as ISubscriber;
            node.Unsubscribe(topic);
            logServer.LogAction("Subscriber " + processName + " Unsubscribe " + topic);
        }

        public void Publish(string processName,int interval,int numberOfEvents,string topic)
        {
            if (!processes.ContainsKey(processName))
                return;
            IPublisher node = Activator.GetObject(typeof(IPublisher),
                processes[processName]) as IPublisher;
            node.Publish(topic, numberOfEvents, interval);
            logServer.LogAction("Publisher " + processName + " Publish " + numberOfEvents
               + " Ontopic " + topic + " Interval " + interval);
        }

    }
}
