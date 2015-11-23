using Broker.Order;
using Broker.Ordering;
using CommonTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Broker
{
    /// <summary>
    ///     Implement all the Broker logic.
    /// </summary>
    public class BrokerLogic : GenericNode
    {
        private string brokerName;
        public string BrokerName { get { return brokerName; } }

        public string SiteName { get; set; }

        private string parentName;
        public string ParentName { get { return parentName; } }

        public bool IsRoot { get; set; }

        private string orderingPolicy;

        private string routingPolicy;

        private string loggingLevel;

        private string pmLogServerUrl;

        private CommonTypes.ThreadPool pool;

        private TopicSubscriberCollection topicSubscribers;
        public TopicSubscriberCollection Data { get { return topicSubscribers; } }

        private IRouter router;

        private IOrder order;

        private IPuppetMasterLog logServer;

        private IBroker remoteProxy;
        public IBroker RemoteProxy { get { return remoteProxy; } }

        private IBroker parentSiteBroker;
        public IBroker ParentSiteBroker { get { return parentSiteBroker; } }

        private Dictionary<string, IBroker> childSites;

        private List<IBroker> brothers = new List<IBroker>();

        public BrokerLogic(IBroker myProxy,string name, string orderingPolicy, string routingPolicy,
            string loggingLevel, string pmLogServerUrl)
        {
            this.remoteProxy = myProxy;
            this.brokerName = name;
            this.pmLogServerUrl = pmLogServerUrl;
            this.orderingPolicy = orderingPolicy;
            this.loggingLevel = loggingLevel;
            this.routingPolicy = routingPolicy;
            if (routingPolicy.Equals("filter"))
            {
                this.router = new Filtered(this);
            }
            else if (routingPolicy.Equals("flooding"))
            {
                this.router = new Flooding(this);
            }
            if (orderingPolicy.Equals("FIFO"))
            {
                this.order = new FifoOrdering();
            }
            else if (orderingPolicy.Equals("NO"))
            {
                this.order = new NoOrdering();
            }
            else if (orderingPolicy.Equals("TOTAL"))
            {
                //TODO
            }
            this.pool = new CommonTypes.ThreadPool(10);
            this.topicSubscribers = new TopicSubscriberCollection();
            childSites = new Dictionary<string, IBroker>();
        }

        // Public methods

        /// <summary>
        /// Basically the Site DTO passed as paramenter has all the information about the broker site.
        /// </summary>
        /// <param name="o"> Site DTO </param>
        public override void Init(Object o)
        {
            SiteDTO site = o as SiteDTO;
            Console.Write(site);
            IsRoot = site.IsRoot;
            SiteName = site.Name;
            if (!IsRoot)
            {
                parentSiteBroker = new BrokerSiteFrontEnd(site.Parent.Brokers, site.Parent.Name);
                parentName = site.Parent.Name;
            }
            else
            {
                parentName = null;
            }
            foreach(BrokerPairDTO dto in site.Brokers)
            {
                brothers.Add(Activator.GetObject(typeof(IBroker), dto.Url) as IBroker);
            }
            foreach(SiteDTO.SiteBrokers dto in site.Childs)
            {
                childSites.Add(dto.Name, new BrokerSiteFrontEnd(dto.Brokers, dto.Name));
            }
            logServer = Activator.GetObject(typeof(IPuppetMasterLog), pmLogServerUrl) as IPuppetMasterLog;
            Console.WriteLine("Broker up and running.......");
        }

        public ICollection<NodePair<IBroker>> GetNeighbours()
        {
            HashSet<NodePair<IBroker>> brokers = new HashSet<NodePair<IBroker>>();
            if (!IsRoot)
            {
                brokers.Add(new NodePair<IBroker>(ParentName, ParentSiteBroker));
            }
            foreach (var pair in childSites)
            {
                brokers.Add(new NodePair<IBroker>(pair.Key, pair.Value));
            }
            return brokers;
        }

        public void AddEventToDiffusion(Event e)
        {
            order.AddNewMessage(e.Publisher, e.SequenceNumber);
            pool.AssyncInvoke(new WaitCallback(Diffuse), e);
        }

        public void AddSubscription(Subscription s)
        {
            pool.AssyncInvoke(new WaitCallback(ProcessSubscribe), s);
        }

        public void AddUnsubscription(Subscription s)
        {
            pool.AssyncInvoke(new WaitCallback(ProcessUnsubscribe), s);
        }

        public void AddRoute(Route r)
        {
            pool.AssyncInvoke(new WaitCallback(ProcessAddRoute), r);
        }

        public void RemoveRoute(Route r)
        {
            pool.AssyncInvoke(new WaitCallback(ProcessRemoveRoute), r);
        }

        public override void Status()
        {
            //TODO
        }

        // Private methods 

        private void Diffuse(Object o)
        {
            this.BlockWhileFrozen();
            Event e = o as Event;

            order.Deliver(e.Publisher, e.SequenceNumber);
            // Send the event to interested Brokers
            if (loggingLevel.Equals("full"))
                logServer.LogAction("BroEvent " + brokerName + " " + e.Publisher + " " + e.Topic + " " + e.SequenceNumber);
            Event newEvent = this.router.Diffuse(e);

            // Send the event to Subscribers who want it
            ICollection<NodePair<ISubscriber>> subscribersToSend = Data.SubscribersFor(e.Topic);

            foreach (var subscriberPair in subscribersToSend)
            {
                subscriberPair.Node.Receive(newEvent);
            }
            order.ConfirmDeliver(e.Publisher);
        }

        private void ProcessUnsubscribe(Object subscription)
        {
            this.BlockWhileFrozen();

            this.router.Unsubscribe(subscription as Subscription);
        }

        private void ProcessSubscribe(Object subscription)
        {
            this.BlockWhileFrozen();

            this.router.Subscribe(subscription as Subscription);
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
