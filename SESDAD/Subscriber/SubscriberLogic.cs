using CommonTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Subscriber
{
    public class SubscriberLogic : GenericNode
    {
        private string name;

        private string pmLogServerUrl;

        private string loggingLevel;

        private string[] brokers;

        private IPuppetMasterLog logServer;

        private CommonTypes.ThreadPool pool;

        private ISubscriber myProxy;
        public ISubscriber MyProxy { get { return myProxy; } }


        public SubscriberLogic(ISubscriber myProxy,string orderingPolicy, string name,
            string pmLogServerUrl, string loggingLevel, string[] brokers)
        {
            if (orderingPolicy.Equals("NO"))
            {
                pool = new CommonTypes.ThreadPool(10);
            }
            else if (orderingPolicy.Equals("FIFO"))
            {
                pool = new CommonTypes.ThreadPool(1);
            }
            else
            {
                //TODO
            }
            this.myProxy = myProxy;
            this.name = name;
            this.pmLogServerUrl = pmLogServerUrl;
            this.loggingLevel = loggingLevel;
            this.brokers = brokers;
            logServer = Activator.GetObject(typeof(IPuppetMasterLog), pmLogServerUrl)
                as IPuppetMasterLog;
        }


        // Public specific methods

        public void Receive(Event e)
        {
            Console.WriteLine("Posto na queue {0}", e.SequenceNumber);
            pool.AssyncInvoke(new WaitCallback(ProcessReceive), e);
        }

        public void Subscribe(string topicName)
        {
            pool.AssyncInvoke(new WaitCallback(ProcessSubscribe), topicName);
        }

        public void Unsubscribe(string topicName)
        {
            pool.AssyncInvoke(new WaitCallback(ProcessUnsubscribe), topicName);
        }

        public override void Init()
        {
            //TODO
        }

        public override void Status()
        {
            //TODO
        }

        // Private methods

        private void ProcessReceive(Object o)
        {
            this.BlockWhileFrozen();

            Event e = o as IMessage as Event;
            Console.WriteLine("Publisher: {0} Topic: {1} SN: {2}", e.Publisher,
                e.Topic, e.GetSequenceNumber());
            logServer.LogAction("SubEvent " + name + ", " + e.Publisher
                + ", " + e.Topic + ", " + e.SequenceNumber);
        }


        private void ProcessUnsubscribe(Object o)
        {
            this.BlockWhileFrozen();

            string topicName = o as string;
            IBroker broker = Activator.GetObject(typeof(IBroker), brokers[0]) as IBroker;
            broker.Unsubscribe(new Subscription(this.name, topicName, this.name, MyProxy));
        }

        private void ProcessSubscribe(Object o)
        {
            this.BlockWhileFrozen();

            string topicName = o as string;
            IBroker broker = Activator.GetObject(typeof(IBroker), brokers[0]) as IBroker;
            broker.Subscribe(new Subscription(this.name, topicName, this.name, MyProxy));
            logServer.LogAction("SubSubscribe " + name + " Subscribe " + topicName);
        }

    }
}
