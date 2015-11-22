using CommonTypes;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Publisher
{
    public class PublisherServer : GenericRemoteNode, IPublisherControlServices, IGeneralControlServices
    {
    
        private PublisherLogic publisher;

        public PublisherServer(string name,string pmLogServerUrl)
        {
            publisher = new PublisherLogic(name, pmLogServerUrl);
        }

        // Publisher remote interface methods

        public void Publish(string topicName, int numberOfEvents, int interval)
        {
            publisher.Publish(topicName, numberOfEvents, interval);
        }

        public void Status()
        {
            publisher.Status();
        }

        public void Init(Object o)
        {
            publisher.Init(o);
        }

        public void Crash()
        {
            publisher.Crash();
        }

        public void Freeze()
        {
            publisher.Freeze();
        }

        public void Unfreeze()
        {
            publisher.Unfreeze();
        }
    }
}
