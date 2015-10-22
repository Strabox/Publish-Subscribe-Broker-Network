using CommonTypes;
using System;
using System.Threading;

namespace Subscriber
{
    public class SubscriberServer : GenericRemoteNode, ISubscriberControlServices,
        ISubscriber
    {
        private string name;

        private string pmLogServerUrl;

        private string loggingLevel;

        private string[] brokers;

        private IPuppetMasterLog logServer;

        public SubscriberServer(string name, string pmLogServerUrl,string loggingLevel,
            string[] brokers)
        {
            this.name = name;
            this.pmLogServerUrl = pmLogServerUrl;
            this.loggingLevel = loggingLevel;
            this.brokers = brokers;
            logServer = Activator.GetObject(typeof(IPuppetMasterLog), pmLogServerUrl)
                as IPuppetMasterLog;
        }

        // Subscriber remote interfaces methods.

        public void Receive(Event e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessReceive), e);
        }
        
        public void Subscribe(string topicName)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessSubscribe),topicName);
        }

        public void Unsubscribe(string topicName)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessUnsubscribe), topicName);
        }

        // Subscriber specific methods
        
        private void ProcessReceive(Object o)
        {
            this.BlockWhileFrozen();

            Event e = o as Event;
            logServer.LogAction("SubEvent " + name + ", " + e.Publisher
                + ", " + e.Topic + ", " + e.EventNumber);
        }

        private void ProcessUnsubscribe(Object o)
        {
            this.BlockWhileFrozen();

            string topicName = o as string;
            IBroker broker = Activator.GetObject(typeof(IBroker), brokers[0]) as IBroker;
            broker.Unsubscribe(new Subscription(this.name, topicName, this.name, this));
        }

        private void ProcessSubscribe(Object o)
        {
            this.BlockWhileFrozen();

            string topicName = o as string;
            IBroker broker = Activator.GetObject(typeof(IBroker), brokers[0]) as IBroker;
            broker.Subscribe(new Subscription(this.name, topicName, this.name, this as ISubscriber));
            logServer.LogAction("SubSubscribe " + name + " Subscribe " + topicName);
        }

        public override void Status()
        {
            //TODO
        }

        public override void Init()
        {
            //Do nothing for now.
        }
    }
}
