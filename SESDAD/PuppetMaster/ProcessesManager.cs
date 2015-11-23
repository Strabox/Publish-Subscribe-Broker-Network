using CommonTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Forms;

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
            RemotingServices.Marshal(logServer, Util.PUPPET_MASTER_NAME,
                typeof(LogServer));
            this.parentForm = form;
        }

        public void LaunchConfigurationFile(string filePath)
        {
            ManageSites sites = new ManageSites();
            ProcessLauncher launcher = new ProcessLauncher();
            string[] tokens = null, lines = File.ReadAllLines(filePath); int l = 1;
            logServer.LogFile = "Log" + Path.GetFileName(filePath);
            File.CreateText(LOG_FILES_DIRECTORY + logServer.LogFile).Close(); 
            parentForm.ReloadLogFiles();

            foreach (string line in lines)
            {
                if (Regex.IsMatch(line, ParseUtil.SITE))
                {
                    tokens = Regex.Split(line, ParseUtil.SPACE);
                    if (!tokens[3].Equals("none"))
                        sites.CreateSiteWithParent(tokens[1], tokens[3]);
                    sites.CreateSite(tokens[1]);
                }
                else if (Regex.IsMatch(line, ParseUtil.PROCESS))
                {
                    tokens = Regex.Split(line, ParseUtil.SPACE);
                    string processType = tokens[3];
                    string port = ParseUtil.ExtractPortFromURL(tokens[7]);
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
                        sites.AddBrokerUrlToSite(tokens[5], tokens[7],tokens[1]);
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
                    launcher.LogLevel = tokens[1];
                }
                else if(line != string.Empty) //Syntax error in a script instruction. Abort.
                {
                    processes.Clear();
                    File.Delete(LOG_FILES_DIRECTORY + logServer.LogFile);
                    parentForm.ReloadLogFiles();
                    MessageBox.Show("Syntax Error at line " + l + " :" + Environment.NewLine + 
                        Environment.NewLine + line , "Script Abort", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                l++;
            }
            //Execute Asynchronous Launching...
            ExecuteBackgroundTask task = new ExecuteBackgroundTask("Booting System"," Initializing", 
                "LaunchAllProcesses", sites, launcher);
            task.ShowDialog();
        }

        public void ExecuteScriptFile(string scriptFilePath,BackgroundWorker worker)
        {
            string[] lines = File.ReadAllLines(scriptFilePath);
            int scriptLines = lines.Length,percentageCompleted = 0,l = 1;
            foreach (string line in lines)
            {
                worker.ReportProgress(percentageCompleted, line);
                if (Regex.IsMatch(line, ParseUtil.PUBLISH))
                {
                    string[] tokens = line.Split(' ');
                    Publish(tokens[1], int.Parse(tokens[7]), int.Parse(tokens[3]),
                        tokens[5]);
                }
                else if (Regex.IsMatch(line, ParseUtil.SUBSCRIBE))
                {
                    string[] tokens = line.Split(' ');
                    Subscribe(tokens[1], tokens[3]);
                }
                else if (Regex.IsMatch(line, ParseUtil.CRASH))
                {
                    string[] tokens = line.Split(' ');
                    Crash(tokens[1]);
                }
                else if (Regex.IsMatch(line, ParseUtil.FREEZE))
                {
                    string[] tokens = line.Split(' ');
                    Freeze(tokens[1]);
                }
                else if (Regex.IsMatch(line, ParseUtil.UNFREEZE))
                {
                    string[] tokens = line.Split(' ');
                    Unfreeze(tokens[1]);
                }
                else if (Regex.IsMatch(line, ParseUtil.STATUS))
                    Status();         
                else if (Regex.IsMatch(line, ParseUtil.WAIT))
                {
                    string[] tokens = line.Split(' ');
                    Thread.Sleep(int.Parse(tokens[1]));
                }
                else if(Regex.IsMatch(line, ParseUtil.UNSUBSCRIBE))
                {
                    string[] tokens = line.Split(' ');
                    Unsubscribe(tokens[1], tokens[3]);
                }
                else if(line != string.Empty) //Syntax error in script line. Abort.
                {
                    MessageBox.Show("Syntax Error at line " + l + " :" + Environment.NewLine +
                        Environment.NewLine + line, "Script Abort", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    worker.CancelAsync();
                    return;
                }
                percentageCompleted += (int)Math.Ceiling(((double)1 / (double)scriptLines) * 100);
                percentageCompleted = (percentageCompleted > 100) ? 100 : percentageCompleted;
                l++;
            }
            worker.ReportProgress(percentageCompleted, string.Empty);
        }

        /* ########################## Remote Calls ########################## */

        public void Status()
        {
            foreach (KeyValuePair<string,string> pair in processes)
            {
                IGeneralControlServices node = Activator.GetObject(typeof(IGeneralControlServices), pair.Value)
                    as IGeneralControlServices;
                node.Status();
            }
        }

        public void CrashAll()
        {
            foreach(KeyValuePair<string,string> pair in processes)
            {
                IGeneralControlServices node = Activator.GetObject(typeof(IGeneralControlServices),pair.Value)
                    as IGeneralControlServices;
                logServer.LogAction("Crash " + pair.Key);
                node.Crash();
            }
            processes.Clear();
            parentForm.BeginInvoke(new Enable(parentForm.EnableConfigFiles), true);
            parentForm.BeginInvoke(new Enable(parentForm.EnableScriptFiles), false);
        }

        public void Crash(string processName)
        {
            if (!processes.ContainsKey(processName))
                return;
            IGeneralControlServices node = Activator.GetObject(typeof(IGeneralControlServices), 
                processes[processName]) as IGeneralControlServices;
            logServer.LogAction("Crash " + processName);
            node.Crash();
            processes.Remove(processName);
            if (processes.Count == 0)
            {
                parentForm.BeginInvoke(new Enable(parentForm.EnableConfigFiles), true);
                parentForm.BeginInvoke(new Enable(parentForm.EnableScriptFiles), false);
            }

        }

        public void Freeze(string processName)
        {
            if (!processes.ContainsKey(processName))
                return;
            IGeneralControlServices node = Activator.GetObject(typeof(IGeneralControlServices), 
                processes[processName]) as IGeneralControlServices;
            logServer.LogAction("Freeze " + processName);
            node.Freeze();
        }

        public void Unfreeze(string processName)
        {
            if (!processes.ContainsKey(processName))
                return;
            IGeneralControlServices node = Activator.GetObject(typeof(IGeneralControlServices),
                processes[processName]) as IGeneralControlServices;
            logServer.LogAction("Unfreeze " + processName);
            node.Unfreeze();
        }

        public void Subscribe(string processName,string topic)
        {
            if (!processes.ContainsKey(processName))
                return;
            ISubscriberControlServices node = Activator.GetObject(typeof(ISubscriberControlServices),
                processes[processName]) as ISubscriberControlServices;
            logServer.LogAction("Subscriber " + processName + " Subscribe " + topic);
            node.Subscribe(topic);
        }

        public void Unsubscribe(string processName,string topic)
        {
            if (!processes.ContainsKey(processName))
                return;
            ISubscriberControlServices node = Activator.GetObject(typeof(ISubscriberControlServices),
                processes[processName]) as ISubscriberControlServices;
            logServer.LogAction("Subscriber " + processName + " Unsubscribe " + topic);
            node.Unsubscribe(topic);
        }

        public void Publish(string processName,int interval,int numberOfEvents,string topic)
        {
            if (!processes.ContainsKey(processName))
                return;
            IPublisherControlServices node = Activator.GetObject(typeof(IPublisherControlServices),
                processes[processName]) as IPublisherControlServices;
            logServer.LogAction("Publisher " + processName + " Publish " + numberOfEvents
               + " Ontopic " + topic + " Interval " + interval);
            node.Publish(topic, numberOfEvents, interval);
        }


    }
}
