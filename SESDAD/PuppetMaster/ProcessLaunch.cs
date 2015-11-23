using CommonTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace PuppetMaster
{
    public class ProcessLauncher
    {
        public static string DEFAULT_ROUTING_POLICY = "flooding";
        public static string DEFAULT_LOG_LEVEL = "light";
        public static string DEFAULT_ORDERING_POLICY = "FIFO";

        private string routingPolicy = DEFAULT_ROUTING_POLICY;

        private string logLevel = DEFAULT_LOG_LEVEL;

        private string orderingPolicy = DEFAULT_ORDERING_POLICY;

        private LinkedList<LaunchNode> launchNodes;

        public string RoutingPolicy
        {
            get { return routingPolicy; }
            set { routingPolicy = value; }
        }

        public string LogLevel
        {
            get { return logLevel; }
            set { logLevel = value; }
        }

        public string OrderingPolicy
        {
            get { return orderingPolicy; }
            set { orderingPolicy = value; }
        }

        public ProcessLauncher()
        {
            launchNodes = new LinkedList<LaunchNode>();
        }

        public void AddNode(LaunchNode node)
        {
            launchNodes.AddFirst(node);
        }

        /// <summary>
        /// Launch all the system processes in corresponding nodes.
        /// </summary>
        /// <param name="sites"> Tree topography and information </param>
        /// <param name="worker"></param>
        public void LaunchAllProcesses(ManageSites sites, BackgroundWorker worker)
        {
            int l = 1, percentageCompleted = 0, totalNodes = launchNodes.Count;
            foreach(LaunchNode node in launchNodes)
            {
                node.Launch(sites, OrderingPolicy, RoutingPolicy, LogLevel);
            }
            foreach (LaunchNode node in launchNodes)
            {
                worker.ReportProgress(percentageCompleted, "Initializing");
                node.InitializeProcess(sites);
                percentageCompleted += (int)Math.Ceiling(((double)1 / (double)totalNodes) * 100);
                percentageCompleted = (percentageCompleted > 100) ? 100 : percentageCompleted;
                l++;
            }
            worker.ReportProgress(percentageCompleted, string.Empty);
        }
    }

    public abstract class LaunchNode
    {
        private string name;
        public string Name { get { return name; } }

        private string port;
        public string Port { get { return port; } }

        private string ip;
        public string Ip { get { return ip; } }

        private string site;
        public string Site { get { return site; } }

        public LaunchNode(string name, string ip, string port, string site)
        {
            this.name = name;
            this.ip = ip;
            this.port = port;
            this.site = site;
        }

        protected void LaunchProcess(string processType,string args)
        {
            if (Ip.Equals("localhost") || Ip.Equals("127.0.0.1"))
            {
                if (Util.IsLinux)
                    Process.Start("mono",
                        string.Join(" ", Util.PROJECT_ROOT + processType +
                        Util.EXE_PATH + processType + ".exe", args));
                else
                    Process.Start(Util.PROJECT_ROOT + processType +
                        Util.EXE_PATH + processType, args);
            }
            else
            {
                IPuppetMasterLauncher launcher = Activator.GetObject(
                    typeof(IPuppetMasterLauncher), Util.MakeUrl("tcp",
                    Ip, Util.PUPPET_MASTER_PORT.ToString(), Util.PUPPET_MASTER_NAME))
                    as IPuppetMasterLauncher;
                launcher.LaunchProcess(processType, args);
            }
        }

        public abstract void InitializeProcess(ManageSites sites);

        public void Launch(ManageSites sites, string orderingPolicy,
            string routingPolicy, string logPolicy)
        {
            //Class name should be: Launch.....
            string processType = this.GetType().Name.Substring(6);
            string args = string.Join(" ", Port, Name,
                orderingPolicy, routingPolicy, logPolicy,
                Util.MakeUrl("tcp", Util.GetLocalIPAddress()
                , Util.PUPPET_MASTER_PORT.ToString(), Util.PUPPET_MASTER_NAME));
            LaunchProcess(processType, args);
        }

    }

    public class LaunchBroker : LaunchNode
    {
        public LaunchBroker(string name,string ip, string port, string site) 
            : base(name,ip,port,site)
        {   }

        public override void InitializeProcess(ManageSites sites)
        {
            (Activator.GetObject(typeof(IGeneralControlServices),
            Util.MakeUrl("tcp", Ip, Port, "broker")) as IGeneralControlServices).Init(sites.GetSite(Site).GetSiteDTO());
        }

    }

    public class LaunchPublisher : LaunchNode
    {
        public LaunchPublisher(string name, string ip, string port, string site) 
            : base(name,ip,port,site){ }

        public override void InitializeProcess(ManageSites sites)
        {
            (Activator.GetObject(typeof(IGeneralControlServices),
                Util.MakeUrl("tcp", Ip, Port, "pub")) as IGeneralControlServices).Init(sites.GetSite(Site).GetSiteDTO());
        }

    }

    public class LaunchSubscriber : LaunchNode
    {
        public LaunchSubscriber(string name,string ip, string port, string site) 
            : base(name,ip,port,site){ }

        public override void InitializeProcess(ManageSites sites)
        {
            (Activator.GetObject(typeof(IGeneralControlServices),
                Util.MakeUrl("tcp", Ip, Port, "sub")) as IGeneralControlServices).Init(sites.GetSite(Site).GetSiteDTO());
        }

    }
   
}
