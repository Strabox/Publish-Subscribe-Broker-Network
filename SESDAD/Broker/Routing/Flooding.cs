using System;
using CommonTypes;

namespace Broker
{
    public class Flooding : IRouter
    {
        private BrokerLogic broker;
        
        public Flooding(BrokerLogic broker)
        {
            this.broker = broker;
        }
        
        public void Subscribe(Subscription subscription)
        {
            broker.Data.AddSubscriber(subscription.Topic, subscription.SubscriberName, subscription.Subscriber);
        }
        
        public void Unsubscribe(Subscription subscription)
        {
            broker.Data.RemoveSubscriber(subscription.Topic, subscription.SubscriberName, subscription.Subscriber);
        }

        public Event Diffuse(Event evt)
        {
            Event newEvent = new Event(evt.Publisher, broker.SiteName, evt.Topic, evt.Content,evt.SequenceNumber);
                    
            foreach (var b in this.broker.GetNeighbours())
            {
                if ( ! evt.Sender.Equals(b.Name))
                {
                    b.Node.Diffuse(newEvent);
                }
            }
            
            return newEvent;
        }

        public void AddRoute(Route route)
        {
            //Do Nothing
        }

        public void RemoveRoute(Route route)
        {
            //Do Nothing
        }

        public void DiffuseBludger(Bludger b)
        {
            foreach (var c in this.broker.GetChildren())
            {
                c.Bludger(b);
            }
        }
    }
}