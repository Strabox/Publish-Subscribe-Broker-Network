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
        private class PublishDTO
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
                this.interval = interval;
            }
        }

        private string name;

        private string siteName;

        private int sequenceNumber;

        private CommonTypes.ThreadPool pool;

        private IBroker brokerSite;

        private IPuppetMasterLog logServer;


        public PublisherLogic(string name, string pmLogServerUrl)
        {
            this.sequenceNumber = 0;
            this.name = name;
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

        public override void Init(Object o)
        {
            SiteDTO dto = o as SiteDTO;
            siteName = dto.Name;
            brokerSite = new BrokerSiteFrontEnd(dto.Brokers, dto.Name);
            Console.Write(brokerSite.ToString());
            Console.WriteLine("Publisher Up and running........");
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
            for (int i = 0; i < dto.NumEvents; i++)
            {
                lock (this)
                {
                    sn = sequenceNumber++;
                    logServer.LogAction("PubEvent " + name + ", " + name + ", " + dto.Topic + ", " + sn);
                    brokerSite.Diffuse(new Event(this.name, this.siteName, dto.Topic, "content", sn));
                }
                Thread.Sleep(dto.Interval);
            }
        }

    }
}
