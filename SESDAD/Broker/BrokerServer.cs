using CommonTypes;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Broker
{
    public class BrokerServer : GenericRemoteNode, IBroker
    {
        private string name;
        public string Name { get { return name; } }        

        private string orderingPolicy;

        private string routingPolicy;

        private string loggingLevel;

        private string pmLogServerUrl;

        private string parentUrl;
        public string ParentUrl { get { return parentUrl; } }

        private string parentName;
        public string ParentName { get { return parentName; } }
        
        private string[] childUrls;

        private TopicSubscriberCollection topicSubscribers;
        public TopicSubscriberCollection Data { get { return topicSubscribers; } }
        
        private IRouter router;

        private IPuppetMasterLog logServer;

        private IBroker parentBroker;
        public IBroker ParentBroker { get { return parentBroker; } }
        
        private Dictionary<string, IBroker> childBrokers = new Dictionary<string, IBroker>();
        public Dictionary<string, IBroker> ChildBrokers { get { return childBrokers; } }

        public BrokerServer(string name,string orderingPolicy,string routingPolicy,
            string loggingLevel,string pmLogServerUrl, string parent, string[] children)
        {
            this.name = name;
            this.pmLogServerUrl = pmLogServerUrl;
            this.orderingPolicy = orderingPolicy;
            this.routingPolicy = routingPolicy;
            if (routingPolicy == "filtered")
            {
                this.router = new Filtered(this);
            }
            else
            {
                this.router = new Flooding(this);
            }
            this.loggingLevel = loggingLevel;
            this.parentUrl = parent;
            this.childUrls = children;
            this.topicSubscribers = new TopicSubscriberCollection();
        }

        /**
         * Init is called after launch all processes and before the system start working.
         */
        public override void Init()
        {
            if ( ! parentUrl.Equals(CommonUtil.ROOT))
            {
                parentBroker = Activator.GetObject(typeof(IBroker), parentUrl) as IBroker;
                parentName = CommonUtil.ExtractPath(parentUrl);
            }
            else
            {
                parentBroker = null;
                parentName = null;
            }
            
            foreach (string childUrl in childUrls)
            {
                IBroker childBroker = Activator.GetObject(typeof(IBroker), childUrl) as IBroker;
                childBrokers.Add(CommonUtil.ExtractPath(childUrl), childBroker);
            }
            
            logServer = Activator.GetObject(typeof(IPuppetMasterLog), pmLogServerUrl) as IPuppetMasterLog;
        }
        
        public ICollection<NodePair<IBroker>> GetNeighbours()
        {
            HashSet<NodePair<IBroker>> brokers = new HashSet<NodePair<IBroker>>();
            if (this.parentBroker != null)
            {
                brokers.Add(new NodePair<IBroker>(ParentName, ParentBroker));
            }
            foreach (var pair in childBrokers)
            {
                brokers.Add(new NodePair<IBroker>(pair.Key, pair.Value));
            }
            return brokers;
        }

        // Broker remote interface methods

        public override void Status()
        {
            //TODO
        }

        public void Diffuse(Event e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessDiffuse), e);
        }

        public void Subscribe(Subscription subscription)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessSubscribe), subscription);
        }

        public void Unsubscribe(Subscription subscription)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessUnsubscribe), subscription);
        }

        // Broker specific Methods

        private void ProcessDiffuse(Object eve)
        {
            this.BlockWhileFrozen();

            Event e = eve as Event;

            // Send the event to interested Brokers
            Event newEvent = this.router.Diffuse(e);

            // Send the event to Subscribers who want it
            ICollection<NodePair<ISubscriber>> subscribersToSend = Data.SubscribersFor(e.Topic);

            foreach (var subscriberPair in subscribersToSend)
            {
                subscriberPair.Node.Receive(newEvent);
            }
            if(loggingLevel.Equals("Full"))
                logServer.LogAction("BroEvent " + name + ", " + e.Publisher
                     + ", " + e.Topic + ", " + e.EventNumber);
        }

        public void ProcessUnsubscribe(Object subscription)
        {
            this.BlockWhileFrozen();
            
            this.router.Unsubscribe(subscription as Subscription);
        }

        private void ProcessSubscribe(Object subscription)
        {
            this.BlockWhileFrozen();
            
            this.router.Subscribe(subscription as Subscription);
        }
        
        public void AddRoute(Route route)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessAddRoute), route);
        }

        public void RemoveRoute(Route route)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessAddRoute), route);
        }
        
        private void ProcessAddRoute(Object route)
        {
            this.BlockWhileFrozen();
            
            this.router.AddRoute(route as Route);
        }
        
        private void ProcessRemoveRoute(Object route)
        {
            this.BlockWhileFrozen();
            
            this.router.RemoveRoute(route as Route);
        }

    }
}
