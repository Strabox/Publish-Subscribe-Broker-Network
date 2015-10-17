﻿using CommonTypes;
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
            RemotingServices.Marshal(logServer, CommonUtil.PUPPET_MASTER_NAME,
                typeof(LogServer));
            this.parentForm = form;
        }

        public void LaunchConfigurationFile(string filePath)
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
                    launcher.LogLevel = tokens[1];
                }
            }
            launcher.LaunchAllProcesses(sites); 
        }

        public void ExecuteScriptFile(string scriptFilePath,BackgroundWorker worker)
        {
            string[] lines = File.ReadAllLines(scriptFilePath);
            int scriptLines = lines.Length,percentageCompleted = 0;
            foreach (string line in lines)
            {
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
                percentageCompleted += (int)Math.Ceiling(((double)1 / (double)scriptLines) * 100);
                percentageCompleted = (percentageCompleted > 100) ? 100 : percentageCompleted;
                worker.ReportProgress(percentageCompleted);
            }
        }
        /* ########################## Remote Calls ########################## */

        public void Status()
        {
            foreach(KeyValuePair<string,string> pair in processes)
            {
                IGeneralControlServices node = Activator.GetObject(typeof(IGeneralControlServices), pair.Value)
                    as IGeneralControlServices;
                node.Status();
            }
            logServer.LogAction("Status");
        }

        public void CrashAll()
        {
            foreach(KeyValuePair<string,string> pair in processes)
            {
                IGeneralControlServices node = Activator.GetObject(typeof(IGeneralControlServices),pair.Value)
                    as IGeneralControlServices;
                node.Crash();
                logServer.LogAction("Crash " + pair.Key);
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
            node.Crash();
            logServer.LogAction("Crash " + processName);
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
            node.Freeze();
            logServer.LogAction("Freeze " + processName);
        }

        public void Unfreeze(string processName)
        {
            if (!processes.ContainsKey(processName))
                return;
            IGeneralControlServices node = Activator.GetObject(typeof(IGeneralControlServices),
                processes[processName]) as IGeneralControlServices;
            node.Unfreeze();
            logServer.LogAction("Unfreeze " + processName);
        }

        public void Subscribe(string processName,string topic)
        {
            if (!processes.ContainsKey(processName))
                return;
            ISubscriberControlServices node = Activator.GetObject(typeof(ISubscriberControlServices),
                processes[processName]) as ISubscriberControlServices;
            node.Subscribe(topic);
            logServer.LogAction("Subscriber " + processName + " Subscribe " + topic);
        }

        public void Unsubscribe(string processName,string topic)
        {
            if (!processes.ContainsKey(processName))
                return;
            ISubscriberControlServices node = Activator.GetObject(typeof(ISubscriberControlServices),
                processes[processName]) as ISubscriberControlServices;
            node.Unsubscribe(topic);
            logServer.LogAction("Subscriber " + processName + " Unsubscribe " + topic);
        }

        public void Publish(string processName,int interval,int numberOfEvents,string topic)
        {
            if (!processes.ContainsKey(processName))
                return;
            IPublisherControlServices node = Activator.GetObject(typeof(IPublisherControlServices),
                processes[processName]) as IPublisherControlServices;
            node.Publish(topic, numberOfEvents, interval);
            logServer.LogAction("Publisher " + processName + " Publish " + numberOfEvents
               + " Ontopic " + topic + " Interval " + interval);
        }

    }
}
