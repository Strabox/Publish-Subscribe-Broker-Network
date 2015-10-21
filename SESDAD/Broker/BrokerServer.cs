using CommonTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Broker
{
    public class BrokerServer : MarshalByRefObject, IGeneralControlServices, IBroker
    {
        private string name;

        private string orderingPolicy;

        private string routingPolicy;

        private string loggingLevel;

        private string pmLogServerUrl;

        private string parentUrl;

        private string parentName;
        
        private string[] childUrls;

        private bool isFreeze;
        private bool IsFreeze
        {
            get { return isFreeze; }
            set { isFreeze = value; }
        }

        private TopicSubscriberCollection topicSubscribers;

        private IPuppetMasterLog logServer;

        private IBroker parentBroker;

        private Dictionary<string, IBroker> childBrokers = new Dictionary<string, IBroker>();

        public BrokerServer(string name,string orderingPolicy,string routingPolicy,
            string loggingLevel,string pmLogServerUrl, string parent, string[] children)
        {
            this.name = name;
            this.pmLogServerUrl = pmLogServerUrl;
            this.orderingPolicy = orderingPolicy;
            this.routingPolicy = routingPolicy;
            this.loggingLevel = loggingLevel;
            this.parentUrl = parent;
            this.childUrls = children;
            this.isFreeze = false;
            this.topicSubscribers = new TopicSubscriberCollection();
        }

        //Init is called after launch all processes and before the system start working.
        public void Init()
        {
            if (!parentUrl.Equals(CommonUtil.ROOT))
            {
                parentBroker = Activator.GetObject(typeof(IBroker), parentUrl)
                    as IBroker;
                parentName = CommonUtil.ExtractPath(parentUrl);
            }
            foreach (string childUrl in childUrls)
            {
                IBroker childBroker = Activator.GetObject(typeof(IBroker), childUrl)
                    as IBroker;
                childBrokers.Add(CommonUtil.ExtractPath(childUrl), childBroker);
            }
            logServer = Activator.GetObject(typeof(IPuppetMasterLog), pmLogServerUrl)
                as IPuppetMasterLog;
        }

        // Broker specific methods.

        private void ProcessDiffuse(Object eve)
        {
            lock (this)
            {
                while (IsFreeze)
                    Monitor.Wait(this);
            }

            Event e = eve as Event;
            // TODO - On Progress
            // Enviar aos Brokers vizinhos.
            Event newEvent = new Event(e.Publisher, this.name, e.Topic, e.Content, e.EventNumber);

            if (!parentUrl.Equals(CommonUtil.ROOT) && !e.Sender.Equals(parentName))
            {
                parentBroker.Diffuse(newEvent);
            }

            if (loggingLevel.Equals("full"))
                logServer.LogAction("BroEvent " + name + ", " + e.Publisher + ", "
                    + newEvent.Topic + ", " + e.EventNumber);

            foreach (KeyValuePair<string, IBroker> child in childBrokers)
            {
                if (!e.Sender.Equals(child.Key))
                    child.Value.Diffuse(newEvent);
            }
            // Send to subscribers that want the message.
            ICollection<ISubscriber> subscribersToSend = topicSubscribers.SubscribersFor(e.Topic);
            foreach (ISubscriber subscriber in subscribersToSend)
            {
                Console.WriteLine(subscribersToSend.Count);
                subscriber.Receive(newEvent);
            }
        }

        public void Diffuse(Event e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessDiffuse),e);
        }

        public void Subscribe(Subscription subscription)
        {
            lock (this)
            {
                this.topicSubscribers.AddSubscriber(subscription.Topic, subscription.Subscriber);
            }
        }

        public void Unsubscribe(Subscription subscription)
        {
            lock (this)
            {
                this.topicSubscribers.RemoveSubscriber(subscription.Topic, subscription.Subscriber);
            }
            string sender = subscription.Sender;
            subscription.Sender = this.name;

            if (!parentUrl.Equals(CommonUtil.ROOT) && !parentName.Equals(sender))
                parentBroker.Unsubscribe(subscription);

            foreach (KeyValuePair<string, IBroker> childPair in childBrokers)
            {
                IBroker child = childPair.Value;
                if (!CommonUtil.ExtractPath(childPair.Key).Equals(sender))
                        child.Unsubscribe(subscription);
            }

        }

        // General test and control methods.

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

      
    }
}
