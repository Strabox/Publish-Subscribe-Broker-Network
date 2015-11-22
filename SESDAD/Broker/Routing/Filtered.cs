using System;
using System.Collections.Generic;
using CommonTypes;

namespace Broker
{
    public class Filtered : IRouter
    {
        private BrokerLogic broker;

        public Filtered(BrokerLogic broker)
        {
            this.broker = broker;
        }

        public void Subscribe(Subscription subscription)
        {
            bool inform = broker.Data.AddSubscriber(subscription.Topic, subscription.SubscriberName, subscription.Subscriber);

            if (inform)
            {
                Route route = new Route(subscription.Topic, broker.SiteName, broker.RemoteProxy);

                foreach (var b in broker.GetNeighbours())
                {
                    b.Node.AddRoute(route);
                }
            }
           
        }

        public Event Diffuse(Event e)
        {
            Event newEvent = new Event(e.Publisher, broker.SiteName, e.Topic, e.Content,e.SequenceNumber);

            ICollection<NodePair<IBroker>> brokersToSend = broker.Data.RoutingFor(e.Topic);

            foreach (NodePair<IBroker> brokerPair in brokersToSend)
            {
                if (!brokerPair.Name.Equals(e.Sender))
                {
                    brokerPair.Node.Diffuse(newEvent);
                }
            }

            return newEvent;
        }

        public void Unsubscribe(Subscription subscription)
        {
            bool inform = broker.Data.RemoveSubscriber(subscription.Topic, subscription.SubscriberName, subscription.Subscriber);

            if (inform)
            {
                Route route = new Route(subscription.Topic, broker.SiteName, broker.RemoteProxy);

                foreach (var b in broker.GetNeighbours())
                {
                        b.Node.AddRoute(route);

                }
            }
        }

        public void AddRoute(Route route)
        {
            bool inform = broker.Data.AddRoute(route.Topic, route.SiteName, route.Broker);

            if (inform)
            {
                Route newRoute = new Route(route.Topic, broker.SiteName, broker.RemoteProxy);

                foreach (var b in broker.GetNeighbours())
                {
                    if (!b.Name.Equals(route.SiteName))
                    {
                        b.Node.AddRoute(newRoute);
                    }
                }
            }
        }

        public void RemoveRoute(Route route)
        {
            bool inform = broker.Data.RemoveRoute(route.Topic, route.SiteName, route.Broker);

            if (inform)
            {
                Route newRoute = new Route(route.Topic, broker.SiteName, broker.RemoteProxy);

                foreach (var b in broker.GetNeighbours())
                {
                    if (!b.Name.Equals(route.SiteName))
                    {
                        b.Node.RemoveRoute(newRoute);
                    }
                }
            }
        }
    }
}