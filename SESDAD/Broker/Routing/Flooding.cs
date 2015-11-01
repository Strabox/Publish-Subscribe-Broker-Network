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
            Event newEvent = new Event(evt.Publisher, broker.Name, evt.Topic, evt.Content,evt.SequenceNumber);
                    
            foreach (var broker in this.broker.GetNeighbours())
            {
                if ( ! evt.Sender.Equals(broker.Name))
                {
                    broker.Node.Diffuse(newEvent);
                }
            }
            
            return newEvent;
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