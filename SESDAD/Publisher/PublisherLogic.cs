using CommonTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Publisher
{
    public class PublisherLogic : GenericNode
    {
        class PublishDTO
        {
            private string topic;
            public string Topic { get { return topic; } }

            private int numEvents;
            public int NumEvents { get { return numEvents; } }

            private int interval;
            public int Interval { get { return interval; } }

            public PublishDTO(string topic, int numEvents, int interval)
            {
                this.topic = topic;
                this.numEvents = numEvents;
                this.interval = Interval;
            }
        }

        private string name;

        private string pmLogServerUrl;

        private string loggingLevel;

        private string ordering;

        private string[] brokers;

        private int sequenceNumber;

        private IPuppetMasterLog logServer;

        private CommonTypes.ThreadPool pool;


        public PublisherLogic(string name, string pmLogServerUrl, string loggingLevel
            , string ordering, string[] brokers)
        {
            this.ordering = ordering;
            this.sequenceNumber = 0;
            this.name = name;
            this.pmLogServerUrl = pmLogServerUrl;
            this.loggingLevel = loggingLevel;
            this.brokers = brokers;
            pool = new CommonTypes.ThreadPool(10);
            logServer = Activator.GetObject(typeof(IPuppetMasterLog), pmLogServerUrl)
                as IPuppetMasterLog;
        }

        // Public specific methods

        public void Publish(string topicName, int numberOfEvents, int interval)
        {
            PublishDTO dto = new PublishDTO(topicName, numberOfEvents, interval);
            pool.AssyncInvoke(new WaitCallback(ProcessPublish), dto);
        }

        public override void Init()
        {
            //Do Nothing
        }

        public override void Status()
        {
            //TODO
        }

        // Private methods 

        private void ProcessPublish(Object o)
        {
            this.BlockWhileFrozen();
            int sn;
            PublishDTO dto = o as PublishDTO;
            IBroker broker;
            for (int i = 0; i < dto.NumEvents; i++)
            {
                broker = Activator.GetObject(typeof(IBroker), brokers[0]) as IBroker;
                lock (this)
                {
                    sn = sequenceNumber++;
                    broker.Diffuse(new Event(this.name, this.name, dto.Topic, "content", sn));
                }
                logServer.LogAction("PubEvent " + name + ", " + name + ", " + dto.Topic + ", " + sn);
                Thread.Sleep(dto.Interval);
            }
        }

    }
}
