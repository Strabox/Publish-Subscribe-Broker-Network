using CommonTypes;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Broker
{
    public class BrokerServer : MarshalByRefObject, IGeneralControlServices, IBroker
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

        private bool isFreeze;
        private bool IsFreeze
        {
            get { return isFreeze; }
            set { isFreeze = value; }
        }

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
            if (routingPolicy == "flooding")
            {
                this.router = new Flooding(this);
            }
            else
            {
                this.router = new Flooding(this);
            }
            this.loggingLevel = loggingLevel;
            this.parentUrl = parent;
            this.childUrls = children;
            this.isFreeze = false;
            this.topicSubscribers = new TopicSubscriberCollection();
        }

        /**
         * Init is called after launch all processes and before the system start working.
         */
        public void Init()
        {
            if (!parentUrl.Equals(CommonUtil.ROOT))
            {
                parentBroker = Activator.GetObject(typeof(IBroker), parentUrl) as IBroker;
                parentName = CommonUtil.ExtractPath(parentUrl);
            }
            
            foreach (string childUrl in childUrls)
            {
                IBroker childBroker = Activator.GetObject(typeof(IBroker), childUrl) as IBroker;
                childBrokers.Add(CommonUtil.ExtractPath(childUrl), childBroker);
            }
            
            logServer = Activator.GetObject(typeof(IPuppetMasterLog), pmLogServerUrl) as IPuppetMasterLog;
        }

        // Broker specific methods.

        private void ProcessDiffuse(Object eve)
        {
            this.BlockWhileFrozen();

            Event e = eve as Event;

            // Send the event to interested Brokers
            Event newEvent = this.router.Diffuse(e);
            
            // Send the event to Subscribers who want it
            ICollection<ISubscriber> subscribersToSend = Data.SubscribersFor(e.Topic);

            foreach (ISubscriber subscriber in subscribersToSend)
            {
                subscriber.Receive(newEvent);
            }
            
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

        public void Crash()
        {
            System.Environment.Exit(-1);
        }

        public void Freeze()
        {
            lock (this)
            {
                IsFreeze = true;
            }
        }

        public void Unfreeze()
        {
            lock (this)
            {
                IsFreeze = false;
                Monitor.PulseAll(this);
            }
        }

        public void Status()
        {
            //TODO
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        private void BlockWhileFrozen()
        {
            lock (this)
            {
                while (IsFreeze)
                    Monitor.Wait(this);
            }
        }
    }
}
