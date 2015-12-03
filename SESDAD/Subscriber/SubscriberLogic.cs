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

        private string loggingLevel;

        private CommonTypes.ThreadPool pool;

        private IBroker brokerSite;

        private IPuppetMasterLog logServer;

        private ISubscriber myProxy;
        public ISubscriber MyProxy { get { return myProxy; } }

        private List<Event> freezer;

        private string ordering;

        private IDetectMessagesRepeated repeated;

        public SubscriberLogic(ISubscriber myProxy,string orderingPolicy, string name,
            string pmLogServerUrl, string loggingLevel)
        {
            pool = new CommonTypes.ThreadPool(1);
            this.myProxy = myProxy;
            this.name = name;
            this.loggingLevel = loggingLevel;
            this.ordering = orderingPolicy;
            this.freezer = new List<Event>();
            if (orderingPolicy.Equals("FIFO"))
            {
                this.repeated = new DetectRepeatedFIFO();
            }
            else if (orderingPolicy.Equals("NO"))
            {
                this.repeated = new DetectRepeatedMessageNO();
            }
            logServer = Activator.GetObject(typeof(IPuppetMasterLog), pmLogServerUrl)
                as IPuppetMasterLog;
        }


        // Public specific methods

        public override void Init(Object o)
        {
            SiteDTO siteDto = o as SiteDTO;

            brokerSite = new BrokerSiteFrontEnd(siteDto.Brokers, siteDto.Name);
            Console.Write(brokerSite.ToString());
            Console.WriteLine("Subscriber up and running........");
        }

        public override void Status()
        {
            //TODO
        }

        public void Receive(Event e)
        {
            this.BlockWhileFrozen();

            lock (this)
            {
                if (ordering.Equals("TOTAL"))
                {
                    freezer.Add(e);
                }
                else
                {
                    if (!repeated.IsRepeated(e.SequenceNumber, e.Publisher))
                    {
                        Console.WriteLine("Publisher: {0} Topic: {1} SN: {2}", e.Publisher, e.Topic, e.GetSequenceNumber());
                        logServer.LogAction("SubEvent " + name + " " + e.Publisher + " " + e.Topic + " " + e.SequenceNumber);
                    }
                }
            }
        }

        public void Bludger(Bludger bludger)
        {
            this.BlockWhileFrozen();
            lock (this)
            {
                foreach (var e in freezer)
                {
                    if (e.SequenceNumber == bludger.Sequence && e.Publisher == bludger.Publisher)
                    {
                        Console.WriteLine("Publisher: {0} Topic: {1} SN: {2}", e.Publisher, e.Topic, e.GetSequenceNumber());
                        logServer.LogAction("SubEvent " + name + " " + e.Publisher + " " + e.Topic + " " + e.SequenceNumber);
                        return;
                    }
                }
            }
        }

        public void Subscribe(string topicName)
        {
            pool.AssyncInvoke(new WaitCallback(ProcessSubscribe), topicName);
        }

        public void Unsubscribe(string topicName)
        {
            pool.AssyncInvoke(new WaitCallback(ProcessUnsubscribe), topicName);
        }

        // Private methods

        private void ProcessReceive(Object o)
        {
            this.BlockWhileFrozen();

            Event e = o as Event;
            Console.WriteLine("Publisher: {0} Topic: {1} SN: {2}", e.Publisher, e.Topic, e.GetSequenceNumber());
            logServer.LogAction("SubEvent " + name + " " + e.Publisher + " " + e.Topic + " " + e.SequenceNumber);
        }


        private void ProcessUnsubscribe(Object o)
        {
            this.BlockWhileFrozen();

            string topicName = o as string;
            //TODO - I think its missing a log call here.
            brokerSite.Unsubscribe(new Subscription(this.name, topicName, this.name, MyProxy));
        }

        private void ProcessSubscribe(Object o)
        {
            this.BlockWhileFrozen();

            string topicName = o as string;
            logServer.LogAction("SubSubscribe " + name + " Subscribe " + topicName);
            brokerSite.Subscribe(new Subscription(this.name, topicName, this.name, MyProxy));
        }

    }
}
