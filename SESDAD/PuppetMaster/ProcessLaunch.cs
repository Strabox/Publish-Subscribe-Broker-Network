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
    public class ProcessLauncher
    {
        public static string DEFAULT_ROUTING_POLICY = "flooding";
        public static string DEFAULT_LOG_POLICY = "FIFO";
        public static string DEFAULT_ORDERING_POLICY = "light";

        private string routingPolicy = DEFAULT_ROUTING_POLICY;

        private string logPolicy = DEFAULT_LOG_POLICY;

        private string orderingPolicy = DEFAULT_ORDERING_POLICY;

        private LinkedList<LaunchNode> launchNodes;

        public string RoutingPolicy
        {
            get
            {
                return routingPolicy;
            }
            set
            {
                routingPolicy = value;
            }
        }

        public string LogPolicy
        {
            get
            {
                return logPolicy;
            }
            set
            {
                logPolicy = value;
            }
        }

        public string OrderingPolicy
        {
            get
            {
                return orderingPolicy;
            }
            set
            {
                orderingPolicy = value;
            }
        }

        public ProcessLauncher()
        {
            launchNodes = new LinkedList<LaunchNode>();
        }

        public void AddNode(LaunchNode node)
        {
            launchNodes.AddLast(node);
        }

        public void LaunchAllProcesses(ManageSites sites)
        {
            foreach(LaunchNode node in launchNodes)
            {
                node.Launch(sites,OrderingPolicy,RoutingPolicy,LogPolicy);
            }
        }
    }

    public abstract class LaunchNode
    {
        private string name;

        private string port;

        private string ip;

        private string site;

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string Ip
        {
            get
            {
                return ip;
            }
        }

        public string Port
        {
            get
            {
                return port;
            }
        }

        public string Site
        {
            get
            {
                return site;
            }
        }

        public LaunchNode(string name, string ip, string port, string site)
        {
            this.name = name;
            this.ip = ip;
            this.port = port;
            this.site = site;
        }

        protected void LaunchRemoteProcess(string name,string args)
        {
            IPuppetMasterLauncher launcher = Activator.GetObject(
                    typeof(IPuppetMasterLauncher), "tcp://" + Ip + ":" + Port + '/' +
                    CommonConstants.PUPPET_MASTER_NAME) as IPuppetMasterLauncher;
            launcher.LaunchProcess(Name, args);
        }

        public abstract void Launch(ManageSites sites,string orderingPolicy
            ,string routingPolicy,string logPolicy);

    }

    public abstract class LaunchEndNode : LaunchNode
    {
        public LaunchEndNode(string name,string ip, string port, string site) 
            : base(name,ip,port,site)
        { }

        public override void Launch(ManageSites sites, string orderingPolicy,
            string routingPolicy, string logPolicy)
        {
            string args = string.Join(" ", Port, Name,
                orderingPolicy,routingPolicy,logPolicy,
                sites.GetSiteByName(Site).GetBrokersUrl());
            string processType = this.GetType().Name.Substring(6);
            if (Ip.Equals("localhost") || Ip.Equals("127.0.0.1"))
                Process.Start(CommonConstants.PROJECT_ROOT + processType +
                    CommonConstants.EXE_PATH + processType, args);
            else
                LaunchRemoteProcess(processType, args);
        }
    }

    public class LaunchBroker : LaunchNode
    {
        public LaunchBroker(string name,string ip, string port, string site) 
            : base(name,ip,port,site)
        {   }

        public override void Launch(ManageSites sites, string orderingPolicy
            , string routingPolicy, string logPolicy)
        {
            string temp = string.Join(" ", Port, Name,
                orderingPolicy,routingPolicy,logPolicy);
            string parent = "NoParent";
            string children = sites.GetSiteByName(Site).GetChildUrl();
            if (sites.GetSiteByName(Site).Parent != null)
                parent = sites.GetSiteByName(Site).Parent.GetBrokersUrl();
            string args = string.Join(" ",temp, parent, children);
            if (Ip.Equals("localhost") || Ip.Equals("127.0.0.1"))
                Process.Start(CommonConstants.PROJECT_ROOT + "Broker" +
                    CommonConstants.EXE_PATH + "Broker",args);
            else
                LaunchRemoteProcess("Broker", args);
        }
    }

    public class LaunchPublisher : LaunchEndNode
    {
        public LaunchPublisher(string name,string ip, string port, string site) 
            : base(name,ip,port,site){ }
    }

    public class LaunchSubscriber : LaunchEndNode
    {
        public LaunchSubscriber(string name,string ip, string port, string site) 
            : base(name,ip,port,site){ }
    }
   
}
