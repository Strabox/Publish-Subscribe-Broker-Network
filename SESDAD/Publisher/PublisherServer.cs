using CommonTypes;
using System;
using System.Threading;

namespace Publisher
{
    public class PublisherServer : GenericRemoteNode, IPublisherControlServices
    {
        class PublishDTO
        {
            private string topic;
            public string Topic { get { return topic; } }

            private int numEvents;
            public int NumEvents {  get { return numEvents; } }

            private int interval;
            public int Interval { get { return interval; } }

            public PublishDTO(string topic,int numEvents,int interval)
            {
                this.topic = topic;
                this.numEvents = numEvents;
                this.interval = Interval;
            }
        }

        private string name;

        private string pmLogServerUrl;

        private string loggingLevel;

        private string[] brokers;

        private IPuppetMasterLog logServer;

        public PublisherServer(string name,string pmLogServerUrl,string loggingLevel,
            string[] brokers)
        {
            this.name = name;
            this.pmLogServerUrl = pmLogServerUrl;
            this.loggingLevel = loggingLevel;
            this.brokers = brokers;
            logServer = Activator.GetObject(typeof(IPuppetMasterLog), pmLogServerUrl)
                as IPuppetMasterLog;
        }

        // Publisher remote interface methods

        public void Publish(string topicName, int numberOfEvents, int interval)
        {
            PublishDTO dto = new PublishDTO(topicName, numberOfEvents, interval);
            ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessPublish), dto);
        }

        // Publisher specific methods

        public void ProcessPublish(Object o)
        {
            this.BlockWhileFrozen();

            PublishDTO dto = o as PublishDTO;
            IBroker broker;
            for(int i = 0; i < dto.NumEvents; i++)
            {
                broker = Activator.GetObject(typeof(IBroker), brokers[0]) as IBroker;
                broker.Diffuse(new Event(this.name,this.name, dto.Topic, "content",i));
                logServer.LogAction("PubEvent " + name + ", " + name + ", " +
                    dto.Topic + ", " + i);
                Thread.Sleep(dto.Interval);
            }
        }

        public override void Status()
        {
            //TODO
        }

        public override void Init()
        {
            //DO Nothing
        }
    }
}
