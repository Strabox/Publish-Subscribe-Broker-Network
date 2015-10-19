using CommonTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Broker
{
    public class BrokerServer : MarshalByRefObject, IGeneralControlServices, IBroker
    {
        private string name;

        private string url;

        private string orderingPolicy;

        private string routingPolicy;

        private string loggingLevel;

        private string pmLogServerUrl;

        private string parentUrl;

        private string parentName;

        private IBroker parentBroker;
        
        private string[] childUrls;

        private IPuppetMasterLog logServer;

        private TopicSubscriberCollection topicSubscribers;

        private Dictionary<string, IBroker> childBrokers = new Dictionary<string, IBroker>();

        public BrokerServer(string name,string orderingPolicy,string routingPolicy,
            string loggingLevel,string pmLogServerUrl, string url, string parent, string[] children)
        {
            this.name = name;
            this.pmLogServerUrl = pmLogServerUrl;
            this.orderingPolicy = orderingPolicy;
            this.routingPolicy = routingPolicy;
            this.loggingLevel = loggingLevel;
            this.parentUrl = parent;
            this.childUrls = children;
            this.url = url;

            //PROBLEMS
            if (parentName != "NoParent")
            {
                parentBroker = Activator.GetObject(typeof(IBroker), parentUrl)
                    as IBroker;

                parentName = parentBroker.GetName();

                parentBroker.HeyDaddy(this.url);
            }
            //PROBLEMS

            /*
            //if(childUrls)
            foreach (string childUrl in children)
            {
                IBroker childBroker = Activator.GetObject(typeof(IBroker), childUrl)
                    as IBroker;

                childBrokers.Add(childBroker.GetName(), childBroker);
            }*/
            
            this.topicSubscribers = new TopicSubscriberCollection();
            logServer = Activator.GetObject(typeof(IPuppetMasterLog), pmLogServerUrl)
                as IPuppetMasterLog;


        }

        // Broker specific methods.

        public void Diffuse(Event e)
        {
            // TODO - On Progress
            // Enviar aos Brokers vizinhos.
            Event newEvent = new Event(this.name, e.Topic, e.Content);

            if (e.Sender != parentName)
            {
                parentBroker.Diffuse(newEvent);
            }

            foreach(KeyValuePair<string, IBroker> child in childBrokers)
            {
                if (e.Sender != child.Key)
                {
                    child.Value.Diffuse(newEvent);
                }
            }

            // Enviar aos Subscribers que querem a messagem.

            ICollection<ISubscriber> subscribersToSend = topicSubscribers.SubscribersForTopic(e.Topic);

            foreach (ISubscriber subscriber in subscribersToSend)
            {
                subscriber.Receive(newEvent);
            }
             

            if (loggingLevel.Equals("full"))
                logServer.LogAction("BroEvent "+ name +", "+"MissingPublisherName"+", "
                    + newEvent.Topic+", " + "MissingEventNumber");
        }

        public void Subscribe(Subscription subscription)
        {
            this.topicSubscribers.Add(subscription.Topic, subscription.Subscriber);
            // Enviar aos brokers vizinhos

            string sender = subscription.Sender;

            subscription.Sender = this.name;

            if (parentName != sender)
            {
                parentBroker.Subscribe(subscription);
            }
            
            foreach (KeyValuePair<string, IBroker> childPair in childBrokers)
            {
                IBroker child = childPair.Value;
                if (child.GetName() != sender)
                {
                    child.Subscribe(subscription);
                }
            }

            // TODO: LOG

            if (loggingLevel.Equals("full"))
                logServer.LogAction("BroSubscribe " + name + ", " + subscription.Subscriber.GetName() + ", "
                    + subscription.Topic + ", " + "MissingEventNumber");
        }

        public void Unsubscribe(Subscription subscription)
        {
            this.topicSubscribers.Remove(subscription.Topic, subscription.Subscriber);
            // Enviar aos brokers vizinhos

            string sender = subscription.Sender;

            subscription.Sender = this.name;

            if (parentName != sender)
            {
                parentBroker.Unsubscribe(subscription);
            }

            foreach (KeyValuePair<string, IBroker> childPair in childBrokers)
            {
                IBroker child = childPair.Value;
                if (child.GetName() != sender)
                {
                    child.Unsubscribe(subscription);
                }
            }

            // TODO: LOG

            if (loggingLevel.Equals("full"))
                logServer.LogAction("BroUnsubscribe " + name + ", " + subscription.Subscriber.GetName() + ", "
                    + subscription.Topic + ", " + "MissingEventNumber");

        }

        public void HeyDaddy(string url)
        {
            IBroker childBroker = Activator.GetObject(typeof(IBroker), url)
                    as IBroker;

            childBrokers.Add(childBroker.GetName(), childBroker);
        }

        // General test and control methods.

        public void Crash()
        {
            System.Environment.Exit(-1);
        }

        public void Freeze()
        {
            //TODO
        }

        public void Status()
        {
            //TODO
        }

        public void Unfreeze()
        {
            //TODO
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public string GetName()
        {
            return this.name;
        }

    }
}
