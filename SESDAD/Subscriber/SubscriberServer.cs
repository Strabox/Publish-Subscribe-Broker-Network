using CommonTypes;
using System;
using System.Collections.Generic;
using System.Threading;


namespace Subscriber
{
    public class SubscriberServer : GenericRemoteNode, ISubscriberControlServices,
        IGeneralControlServices ,ISubscriber
    {

        private SubscriberLogic subscriber;

        public SubscriberServer(string orderingPolicy, string name, string pmLogServerUrl,string loggingLevel,
            List<string> brokers)
        {
            subscriber = new SubscriberLogic(this, orderingPolicy, name, pmLogServerUrl, loggingLevel, brokers);
        }

        // Subscriber remote interfaces methods.

        public void Receive(Event e)
        {
            subscriber.Receive(e);
        }
        
        public void Subscribe(string topicName)
        {
            subscriber.Subscribe(topicName);
        }

        public void Unsubscribe(string topicName)
        {
            subscriber.Unsubscribe(topicName);
        }

        public void Status()
        {
            subscriber.Status();
        }

        public void Init()
        {
            subscriber.Init();
        }

        public void Crash()
        {
            subscriber.Crash();
        }

        public void Freeze()
        {
            subscriber.Freeze();
        }

        public void Unfreeze()
        {
            subscriber.Unfreeze();
        }
    }
}
