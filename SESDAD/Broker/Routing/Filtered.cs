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

                foreach (var broker in broker.GetNeighbours())
                {
                        broker.Node.AddRoute(route);
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

            return newEvent;
        }

        public void Unsubscribe(Subscription subscription)
        {
            bool inform = broker.Data.RemoveSubscriber(subscription.Topic, subscription.SubscriberName, subscription.Subscriber);

            if (inform)
            {
                Route route = new Route(subscription.Topic, broker.Name, broker);

                foreach (var broker in broker.GetNeighbours())
                {
                        broker.Node.AddRoute(route);
                }
            }
        }

        public void AddRoute(Route route)
        {
            throw new NotImplementedException();
        }

        public void RemoveRoute(Route route)
        {
            throw new NotImplementedException();
        }
    }
}