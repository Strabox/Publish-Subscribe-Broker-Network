using CommonTypes;
using System;
using System.Threading;

namespace Publisher
{
    public class PublisherServer : GenericRemoteNode, IPublisherControlServices, IGeneralControlServices
    {
    
        private PublisherLogic publisher;

        public PublisherServer(string name,string pmLogServerUrl,string loggingLevel
            ,string ordering, string[] brokers)
        {
            publisher = new PublisherLogic(name, pmLogServerUrl, loggingLevel, ordering, brokers);
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

        public void Init()
        {
            publisher.Init();
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
