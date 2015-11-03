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

        private CommonTypes.ThreadPool pool;

        private IBroker remoteProxy;
        public IBroker RemoteProxy { get { return remoteProxy; } }

        private IOrder order;

        public BrokerLogic(IBroker myProxy,string name, string orderingPolicy, string routingPolicy,
            string loggingLevel, string pmLogServerUrl, string parent, string[] children)
        {
            this.remoteProxy = myProxy;
            this.name = name;
            this.pmLogServerUrl = pmLogServerUrl;
            this.orderingPolicy = orderingPolicy;
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
            else if(orderingPolicy.Equals("NO"))
            {
                this.order = new NoOrdering();
            }
            else if (orderingPolicy.Equals("TOTAL"))
            {
                //TODO
            }
            this.loggingLevel = loggingLevel;
            this.parentUrl = parent;
            this.childUrls = children;
            this.pool = new CommonTypes.ThreadPool(10);
            this.topicSubscribers = new TopicSubscriberCollection();
        }

        // Public methods

        public override void Init()
        {
            if (!parentUrl.Equals(CommonUtil.ROOT))
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
            Event newEvent = this.router.Diffuse(e);
            // Send the event to Subscribers who want it
            ICollection<NodePair<ISubscriber>> subscribersToSend = Data.SubscribersFor(e.Topic);

            foreach (var subscriberPair in subscribersToSend)
            {
                subscriberPair.Node.Receive(newEvent);
            }
            order.ConfirmDeliver(e.Publisher);
            if (loggingLevel.Equals("full"))
                logServer.LogAction("BroEvent " + name + ", " + e.Publisher
                     + ", " + e.Topic + ", " + e.SequenceNumber);
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
