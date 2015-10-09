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
using System.Threading.Tasks;

namespace PuppetMaster
{
    public class ProcessesManager
    {

        public static string CONFIG_FILES_DIRECTORY = ".."+
                            Path.DirectorySeparatorChar+
                            ".."+ Path.DirectorySeparatorChar+
                            "ConfigurationFiles" + Path.DirectorySeparatorChar;
        private static string PROJECT_ROOT = ".." + 
                            Path.DirectorySeparatorChar + ".." + 
                            Path.DirectorySeparatorChar + ".." + 
                            Path.DirectorySeparatorChar;
        private static string EXE_PATH = Path.DirectorySeparatorChar+
                            "bin"+ Path.DirectorySeparatorChar + 
                            "Debug"+ Path.DirectorySeparatorChar;

        private FormPuppetMaster parentForm;

        private LogServer logServer;

        // Pair <ProcessName,URL>
        private Dictionary<string, string> processes;


        public ProcessesManager(FormPuppetMaster form)
        {
            processes = new Dictionary<string, string>();
            logServer = new LogServer();
            TcpChannel channel = new TcpChannel(CommonUtil.PUPPET_MASTER_PORT);
            ChannelServices.RegisterChannel(channel, false);
            RemotingServices.Marshal(logServer, CommonUtil.PUPPET_MASTER_NAME, typeof(LogServer));
            this.parentForm = form;
        }

        /* TODO We need to pass the tree structure to the other processes. */
        public void ReadConfigurationFile(string filePath)
        {
            string[] tokens = null, lines = File.ReadAllLines(filePath);
            string loggingLevel = "light", routingPolicy = "flooding",orderingPolicy = "FIFO";
            Dictionary<string, string> sitesAndBrokers = new Dictionary<string, string>(); 
            Dictionary<string, List<string>> sites = new Dictionary<string, List<string>>();

            logServer.LogFile = "Log" + Path.GetFileName(filePath);
            LogManager.CreateLogFile(logServer.LogFile);
            parentForm.ReloadLogFiles();
            foreach (string line in lines)
            {
                if (Regex.IsMatch(line,ParseUtil.SITE))
                {
                    tokens = Regex.Split(line, ParseUtil.SPACE);
                }
                else if (Regex.IsMatch(line,ParseUtil.PROCESS))
                {
                    tokens = Regex.Split(line, ParseUtil.SPACE);
                    string processType = tokens[3].First().ToString().ToUpper() 
                          + tokens[3].Substring(1);
                    string[] urlParse = ParseUtil.ParseURL(tokens[7]);
                    processes.Add(tokens[1], tokens[7]);
                    parentForm.Invoke(new AddProcess(parentForm.AddToGenericProcesses),
                        tokens[1]);
                    if (processType.Equals("Publisher"))
                    {
                        parentForm.Invoke(new AddProcess(parentForm.AddToPublishersProcesses),
                            tokens[1]);
                    }
                    else if (processType.Equals("Subscriber"))
                    {
                        parentForm.Invoke(new AddProcess(parentForm.AddToSubscribersProcesses),
                            tokens[1]);
                    }
                    else if (processType.Equals("Broker"))
                    {
                        sitesAndBrokers.Add(tokens[5],tokens[7]);
                    }
                    Process.Start(PROJECT_ROOT + processType + EXE_PATH + processType,
                        String.Join(" ",urlParse));
                }
                else if (Regex.IsMatch(line,ParseUtil.ROUTING))
                {
                    tokens = Regex.Split(line, ParseUtil.SPACE);
                    routingPolicy = tokens[1];
                }
                else if (Regex.IsMatch(line,ParseUtil.ORDERING))
                {
                    tokens = Regex.Split(line, ParseUtil.SPACE);
                    orderingPolicy = tokens[1];
                }
                else if(Regex.IsMatch(line, ParseUtil.LOGGING_LEVEL))
                {
                    tokens = Regex.Split(line, ParseUtil.SPACE);
                    loggingLevel = tokens[1];
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
               + " Ontopic " + topic + " Inteval " + interval);
        }

    }
}
