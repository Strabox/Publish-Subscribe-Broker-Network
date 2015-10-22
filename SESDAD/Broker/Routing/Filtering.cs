using System;
using System.Collections.Generic;
using CommonTypes;

namespace Broker
{
    public class Filtered : IRouter
    {
        private BrokerServer broker;

        public Filtered(BrokerServer broker)
        {
            this.broker = broker;
        }

        public void Subscribe(Subscription subscription)
        {
            bool inform = broker.Data.AddSubscriber(subscription.Topic, subscription.SubscriberName, subscription.Subscriber);

            if (inform)
            {
               
                Route route = new Route(subscription.Topic, broker.Name, broker);

                if (broker.ParentUrl.Equals(CommonUtil.ROOT))
                {
                    broker.ParentBroker.AddRoute(route);
                }

                foreach (KeyValuePair<string, IBroker> child in broker.ChildBrokers)
                {
                        child.Value.AddRoute(route);
                
                }

            }
           
        }

        public Event Diffuse(Event e)
        {
            Event newEvent = new Event(e.Publisher, broker.Name, e.Topic, e.Content, e.EventNumber);

            ICollection<NodePair<IBroker>> brokersToSend = broker.Data.RoutingFor(e.Topic);

            foreach (NodePair<IBroker> brokerPair in brokersToSend)
            {
                if (!brokerPair.Name.Equals(e.Sender))
                {
                    brokerPair.Node.Diffuse(newEvent);
                }
            }

            /*
            if (loggingLevel.Equals("full"))
                logServer.LogAction("BroEvent " + name + ", " + e.Publisher + ", "
                    + newEvent.Topic + ", " + e.EventNumber);
            */

            return newEvent;
        }

        public void Unsubscribe(Subscription subscription)
        {
            bool inform = broker.Data.RemoveSubscriber(subscription.Topic, subscription.SubscriberName, subscription.Subscriber);

            if (inform)
            {

                Route route = new Route(subscription.Topic, broker.Name, broker);

                if (broker.ParentUrl.Equals(CommonUtil.ROOT))
                {
                    broker.ParentBroker.RemoveRoute(route);
                }

                foreach (KeyValuePair<string, IBroker> child in broker.ChildBrokers)
                {
                    child.Value.RemoveRoute(route);

                }

            }
        }
    }
}