using System;
using System.Collections.Generic;
using CommonTypes;

namespace Broker
{
    public class Flooding : IRouter
    {
        private BrokerServer broker;
        
        public Flooding(BrokerServer broker)
        {
            this.broker = broker;
        }
        
        public void Subscribe(Subscription subscription)
        {
            broker.Data.AddSubscriber(subscription.Topic, subscription.Subscriber);
        }

        public Event Diffuse(Event e)
        {
            Event newEvent = new Event(e.Publisher, broker.Name, e.Topic, e.Content, e.EventNumber);
                        
            if (!broker.ParentUrl.Equals(CommonUtil.ROOT) && !e.Sender.Equals(broker.ParentName))
            {
                broker.ParentBroker.Diffuse(newEvent);
            }

            /*
            if (loggingLevel.Equals("full"))
                logServer.LogAction("BroEvent " + name + ", " + e.Publisher + ", "
                    + newEvent.Topic + ", " + e.EventNumber);
            */

            foreach (KeyValuePair<string, IBroker> child in broker.ChildBrokers)
            {
                if (!e.Sender.Equals(child.Key))
                    child.Value.Diffuse(newEvent);
            }
            
            return newEvent;
        }

        public void Unsubscribe(Subscription subscription)
        {
            broker.Data.RemoveSubscriber(subscription.Topic, subscription.Subscriber);
        }
    }
}